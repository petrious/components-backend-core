using AutoMapper;
using Optsol.Playground.Application.ViewModels.CartaoCredito;
using Optsol.Playground.Domain.Entidades;

namespace Optsol.Playground.Application.Mappers.CartaoCredito
{
    public class CartaoCreditoViewModelToEntityMapper : Profile
    {
        public CartaoCreditoViewModelToEntityMapper()
        {
            CreateMap<InsertCartaoCreditoViewModel, CartaoCreditoEntity>();

            CreateMap<UpdateCartaoCreditoViewModel, CartaoCreditoEntity>();
        }
    }
}
