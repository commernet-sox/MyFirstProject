using AutoMapper;
using CPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Application.Core
{
    public class MapperConf: IMapperConf
    {
        public int Order => 1;

        public void Configure(IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<TestData.RequestEntities.TestApiRequest, TestData.DTO.TestApiDTO>();
            //mapper.CreateMap<EntrustOrderAdd, EntrustOrderDTO>();
            //mapper.CreateMap<EntrustOrderRequest, EntrustOrderDTO>();
            //mapper.CreateMap<EntrustOrderDetail, EntrustOrderDTO>();
            //mapper.CreateMap<EntrustGoodsAdd, EntrustGoodsDTO>();
            //mapper.CreateMap<EntrustGoodsUpdate, EntrustGoodsDTO>();
            //mapper.CreateMap<EntrustOrderUpdate, EntrustOrderDTO>();
            //mapper.CreateMap<EntrustOrderDeparture, EntrustOrderDTO>();
            //mapper.CreateMap<EntrustOrderSign, EntrustOrderDTO>();
            //mapper.CreateMap<EntrustOrderImport, EntrustOrderDTO>();
            //mapper.CreateMap<EntrustOrderImport, EntrustGoodsDTO>();

            //mapper.CreateMap<AssembleAdd, AssembleDTO>();
            //mapper.CreateMap<EntrustOrderDTO, AssembleDTO>();

            //mapper.CreateMap<BillAdd, BillDTO>();
            //mapper.CreateMap<BillUpdate, BillDTO>();

            //mapper.CreateMap<EntrustTrackAddRequest, EntrustTrackDTO>();
            //mapper.CreateMap<EntrustTrackAdd, EntrustTrackDTO>();
        }
    }
}
