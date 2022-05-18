using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WhatsAppApi.Controllers
{
    public class GrupController : ApiController
    {
        SosyalMedyaEntities _ent = new SosyalMedyaEntities();
        GrupKullaniciController gkc = new GrupKullaniciController();
        [HttpGet]
        public List<GrupTip> OturumAcanKullanicininDahilOlduguGruplariGetir(int oakid)
        {
            return _ent.Grup.Where(p => p.GrupKullanici.Any(k => k.KullaniciID == oakid)).
                Select(p => new GrupTip()
                {
                    Adi = p.Adi,
                    GrupID = p.GrupID,
                    OlusturulmaTarihi = p.OlusturulmaTarihi,
                    YoneticiKullaniciID = p.YoneticiKullaniciID,
                    YoneticiKullaniciAdi = p.Kullanici.AdSoyad
                }).ToList();
        }
        [HttpPost]
        public bool YeniGrupOlustur(YeniGrupOlusturmaTipi veri)
        {
            try
            {
                Grup g = new Grup();
                g.Adi = veri.GrupVerisi.Adi;
                g.OlusturulmaTarihi = DateTime.Now;
                g.YoneticiKullaniciID = veri.GrupVerisi.YoneticiKullaniciID;
                _ent.Grup.Add(g);
                _ent.SaveChanges();
                gkc.GrupKullaniciEkle(g.GrupID, veri.GrupVerisi.YoneticiKullaniciID);
                foreach (KullaniciTip item in veri.GrupAltindakiKullanicilar)
                {
                    gkc.GrupKullaniciEkle(g.GrupID, item.KullaniciID);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        [HttpGet]
        public bool GrupSil(int gid, int oakid)
        {
            Grup g = _ent.Grup.Find(gid);
            if (g.YoneticiKullaniciID == oakid)
            {
                using (var transaction = _ent.Database.BeginTransaction())
                {
                    try
                    {
                        // grup kullanıcılar tablosundaki grup ile ilgili kayıtlar temizlendi
                        List<GrupKullanici> silinecekgrupkullanicilar = _ent.GrupKullanici.Where(p => p.GrupID == gid).ToList();
                        _ent.GrupKullanici.RemoveRange(silinecekgrupkullanicilar);
                        _ent.SaveChanges();

                        // silienecek grup altındaki mesajlar temizlendi
                        List<Mesaj> silinecekmesajlar = _ent.Mesaj.Where(p => p.GrupID == gid).ToList();
                        _ent.Mesaj.RemoveRange(silinecekmesajlar);
                        _ent.SaveChanges();

                        // ilişkili veri kalmayınca grup silindi
                        _ent.Grup.Remove(g);
                        _ent.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            return false;
        }

    }
    public class YeniGrupOlusturmaTipi
    {
        public List<KullaniciTip> GrupAltindakiKullanicilar { get; set; }
        public GrupTip GrupVerisi { get; set; }
    }
    public class GrupTip
    {
        public int GrupID { get; set; }
        public string Adi { get; set; }
        public System.DateTime OlusturulmaTarihi { get; set; }
        public int YoneticiKullaniciID { get; set; }
        public string YoneticiKullaniciAdi { get; set; }
    }
}
