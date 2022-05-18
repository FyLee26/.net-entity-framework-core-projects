using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WhatsAppApi.Controllers
{
    public class GrupKullaniciController : ApiController
    {
        SosyalMedyaEntities _ent = new SosyalMedyaEntities();
        public bool GrupKullaniciEkle(int GrupID, int KullaniciID)
        {
            try
            {
                GrupKullanici gk = new GrupKullanici();
                gk.GrupID = GrupID;
                gk.KullaniciID = KullaniciID;
                gk.KatilmaZamani = DateTime.Now;
                _ent.GrupKullanici.Add(gk);
                _ent.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
    public class GrupKullaniciTip
    {
        public int GrupKullaniciID { get; set; }
        public int GrupID { get; set; }
        public int KullaniciID { get; set; }
        public Nullable<System.DateTime> KatilmaZamani { get; set; }
    }
}
