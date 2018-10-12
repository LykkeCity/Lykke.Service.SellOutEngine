using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.SellOutEngine.Domain;
using Lykke.Service.SellOutEngine.Domain.Exceptions;
using Lykke.Service.SellOutEngine.Domain.Extensions;
using Lykke.Service.SellOutEngine.Domain.Repositories;
using Lykke.Service.SellOutEngine.Domain.Services;

namespace Lykke.Service.SellOutEngine.DomainServices.Instruments
{
    public class InstrumentService : IInstrumentService
    {
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly ILykkeExchangeService _lykkeExchangeService;
        private readonly IOrderBookService _orderBookService;
        private readonly InMemoryCache<Instrument> _cache;
        private readonly ILog _log;

        public InstrumentService(
            IInstrumentRepository instrumentRepository,
            ILykkeExchangeService lykkeExchangeService,
            IOrderBookService orderBookService,
            ILogFactory logFactory)
        {
            _instrumentRepository = instrumentRepository;
            _lykkeExchangeService = lykkeExchangeService;
            _orderBookService = orderBookService;
            _cache = new InMemoryCache<Instrument>(instrument => instrument.AssetPairId, false);
            _log = logFactory.CreateLog(this);
        }

        public async Task<IReadOnlyCollection<Instrument>> GetAllAsync()
        {
            IReadOnlyCollection<Instrument> instruments = _cache.GetAll();

            if (instruments == null)
            {
                instruments = await _instrumentRepository.GetAllAsync();

                _cache.Initialize(instruments);
            }

            return instruments;
        }

        public async Task<Instrument> GetByAssetPairIdAsync(string assetPairId)
        {
            IReadOnlyCollection<Instrument> instruments = await GetAllAsync();

            Instrument instrument = instruments.FirstOrDefault(o => o.AssetPairId == assetPairId);

            if (instrument == null)
                throw new EntityNotFoundException();

            return instrument;
        }

        public async Task AddAsync(Instrument instrument, string userId)
        {
            await _instrumentRepository.InsertAsync(instrument);

            _cache.Set(instrument);

            _log.InfoWithDetails("Instrument was added", instrument);
        }

        public async Task UpdateAsync(Instrument instrument, string userId)
        {
            Instrument currentInstrument = await GetByAssetPairIdAsync(instrument.AssetPairId);

            InstrumentMode currentInstrumentMode = currentInstrument.Mode;

            currentInstrument.Update(instrument);

            await _instrumentRepository.UpdateAsync(currentInstrument);

            _cache.Set(currentInstrument);

            if (instrument.Mode == InstrumentMode.Disabled && currentInstrumentMode != InstrumentMode.Disabled)
            {
                try
                {
                    await _lykkeExchangeService.CancelAsync(instrument.AssetPairId);
                }
                catch (Exception exception)
                {
                    _log.WarningWithDetails("An error occurred while cancelling limit orders", exception,
                        currentInstrument);
                }

                await _orderBookService.RemoveAsync(instrument.AssetPairId);
            }

            _log.InfoWithDetails("Instrument was updated", currentInstrument);
        }

        public async Task DeleteAsync(string assetPairId, string userId)
        {
            Instrument instrument = await GetByAssetPairIdAsync(assetPairId);

            if (instrument.Mode == InstrumentMode.Active)
                throw new InvalidOperationException("Can not delete active instrument");

            await _instrumentRepository.DeleteAsync(assetPairId);

            _cache.Remove(assetPairId);

            await _orderBookService.RemoveAsync(instrument.AssetPairId);

            _log.InfoWithDetails("Instrument was deleted", instrument);
        }
    }
}
