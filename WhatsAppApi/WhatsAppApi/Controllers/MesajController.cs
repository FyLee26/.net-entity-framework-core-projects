using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WhatsAppApi.Controllers
{
    public class MesajController : ApiController
    {
        SosyalMedyaEntities _ent = new SosyalMedyaEntities();
        [HttpGet]
        public List<MesajTip> GrupAltindakiMesajlariGetir(int gid, int oakid)
        {
            return _ent.Mesaj.Where(p => p.GrupID == gid).
                Select(p => new MesajTip()
                {
                    MesajID = p.MesajID,
                    MesajMetni = p.MesajMetni,
                    YazanID = p.YazanID,
                    YazanKullaniciAdi = p.YazanID == oakid ? "ben" : p.Kullanici.AdSoyad,
                    MesajZamani = p.MesajZamani
                }).ToList();
            //.OrderByDescending(p=>p.MesajZamani).ToList();
        }
        [HttpPost]
        public bool YeniMesajEkle(MesajTip veri)
        {
            try
            {
                Mesaj m = new Mesaj();
                m.YazanID = veri.YazanID;
                m.GrupID = veri.GrupID;
                m.MesajMetni = veri.MesajMetni;
                m.MesajZamani = DateTime.Now;
                _ent.Mesaj.Add(m);
                _ent.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        [HttpGet]
        public bool MesajSil(int mesajid, int oakid)
        {
            try
            {
                Mesaj m = _ent.Mesaj.Find(mesajid);
                if (m.YazanID == oakid)
                {
                    _ent.Mesaj.Remove(m);
                    _ent.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
    public class MesajTip
    {
        public int MesajID { get; set; }
        public int GrupID { get; set; }
        public int YazanID { get; set; }
        public string YazanKullaniciAdi { get; set; }
        public string MesajMetni { get; set; }
        public System.DateTime MesajZamani { get; set; }

    }
}
