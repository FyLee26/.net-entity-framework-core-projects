using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WhatsAppApi.Controllers
{
    public class KullaniciController : ApiController
    {
        SosyalMedyaEntities _ent = new SosyalMedyaEntities();
        [HttpGet]
        public KullaniciTip KullaniciOturumAc(string mail, string sifre)
        {
            return _ent.Kullanici.Select(p => new KullaniciTip()
            {
                KullaniciID = p.KullaniciID,
                AdSoyad = p.AdSoyad,
                Mail = p.Mail,
                Tel = p.Tel,
                Sifre = p.Sifre
            }).FirstOrDefault(p => p.Mail == mail && p.Sifre == sifre);
        }
        [HttpGet]
        public List<KullaniciTip> OturumAcanKullaniciDisindakiKullanicilariGetir(int oak)
        {
            return _ent.Kullanici.Where(p => p.KullaniciID != oak)
                .Select(p => new KullaniciTip()
                {
                    KullaniciID = p.KullaniciID,
                    AdSoyad = p.AdSoyad,
                    Mail = p.Mail,
                    Tel = p.Tel
                }).ToList();
        }
    }
    public class KullaniciTip
    {
        public int KullaniciID { get; set; }
        public string AdSoyad { get; set; }
        public string Tel { get; set; }
        public string Mail { get; set; }
        public string Sifre { get; set; }
    }
}

