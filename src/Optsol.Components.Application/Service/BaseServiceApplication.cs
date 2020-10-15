﻿using System;
using System.Collections.Generic;
using AutoMapper;
using Optsol.Components.Domain;
using Optsol.Components.Infra.Data;
using Optsol.Components.Shared.Extensions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Infra.UoW;

namespace Optsol.Components.Application.Service
{
    public class BaseServiceApplication<TEntity, TKey> 
        : IBaseServiceApplication<TEntity, TKey>
        where TEntity: class, IAggregateRoot<TKey>
    {
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IReadRepository<TEntity, TKey> _readRepository;
        protected readonly IWriteRepository<TEntity, TKey> _writeRepository;


        public BaseServiceApplication(
            IMapper mapper,
            ILogger<BaseServiceApplication<TEntity, TKey>> logger,
            IUnitOfWork unitOfWork,
            IReadRepository<TEntity, TKey> readRepository, 
            IWriteRepository<TEntity, TKey> writeRepository)
        {
            _logger = logger;
            _logger?.LogInformation($"Inicializando Application Service<{ typeof(TEntity).Name }, { typeof(TKey).Name }>");

            _unitOfWork = unitOfWork ?? throw new UnitOfWorkNullException();
            _mapper = mapper ?? throw new AutoMapperNullException();
            _readRepository = readRepository;
            _writeRepository = writeRepository;
        }

        public async Task<TViewModel> GetByIdAsync<TViewModel>(TKey id)
        {
            _logger?.LogInformation($"Método: { nameof(GetByIdAsync) }({{ id:{ id } }}) Retorno: type { typeof(TViewModel).Name }");
            
            return _mapper.Map<TViewModel>((await _readRepository.GetById(id)));
        }

        public async Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>()
        {
            _logger?.LogInformation($"Método: { nameof(GetAllAsync) }() Retorno: IEnumerable<{ typeof(TViewModel).Name }>");
            
            return (await _readRepository.GetAllAsync()
                .AsyncEnumerableToEnumerable())
                .Select(entity => _mapper.Map<TViewModel>(entity));
        }

        public async Task InsertAsync<TViewModel>(TViewModel viewModel)
        {
            _logger?.LogInformation($"Método: { nameof(InsertAsync) }({{ viewModel:{ viewModel.ToJson() } }})");
            
            var entity = _mapper.Map<TEntity>(viewModel);

            _logger?.LogInformation($"Método: { nameof(InsertAsync) } Mapper: { typeof(TViewModel).Name } To: { typeof(TEntity).Name } Result: { entity.ToJson() }");

            await _writeRepository.InsertAsync(entity);
        }

        public async Task UpdateAsync<TViewModel>(TViewModel viewModel)
        {
            _logger?.LogInformation($"Método: { nameof(UpdateAsync) }({{ viewModel:{ viewModel.ToJson() } }})");
            
            var entity = _mapper.Map<TEntity>(viewModel);

            _logger?.LogInformation($"Método: { nameof(UpdateAsync) } Mapper: { typeof(TViewModel).Name } To: { typeof(TEntity).Name } Result: { entity.ToJson() }");

            await _writeRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(TKey id)
        {
            _logger?.LogInformation($"Método: { nameof(DeleteAsync) }({{ id:{ id.ToString() } }})");

            await _writeRepository.DeleteAsync(id);
        }
    }
}