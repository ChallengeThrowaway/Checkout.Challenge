﻿using AutoMapper;
using PaymentGateway.Core.Models;
using PaymentGateway.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Api.MappingProfiles
{
    public class PaymentMappingProfile : Profile
    {
        public PaymentMappingProfile()
        {
            CreateMap<Models.PaymentRequest, PaymentRequest>();
            CreateMap<PaymentRequest, Models.PaymentRequest>();

            CreateMap<PaymentRequest, CardDetails>();
            CreateMap<PaymentRequest, Payment>()
                .ForMember(dest => dest.CardDetails, opt => opt.MapFrom( src => src));
        }
    }
}