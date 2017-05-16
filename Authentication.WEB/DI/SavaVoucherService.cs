using AutoMapper;
using InsuredTraveling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class SavaVoucherService : ISavaVoucherService
    {
        InsuredTravelingEntity2 _db = new InsuredTravelingEntity2();
        public void AddSavaVoucher(SavaVoucherModel SavaVoucher)
        {
            sava_voucher SavaVoucher2 = _db.sava_voucher.Create();
            SavaVoucher2 = Mapper.Map<SavaVoucherModel, sava_voucher>(SavaVoucher);
            _db.sava_voucher.Add(SavaVoucher2);
            _db.SaveChanges();
        }
    }
}