var app = angular.module("SosyalMedyaApp", ["ngRoute"]);
app.config(function ($routeProvider) {
    $routeProvider
    .when("/", {
        templateUrl: "login.html",
        controller: "loginCtrl"
    })
    .when("/grup", {
        templateUrl: "GrupIslemleri.html",
        controller: "grupIslemleriCtrl"
    })
    .when("/mesaj", {
        templateUrl: "MesajIslemleri.html",
        controller: "mesajIslemleriCtrl"
    });
});
app.controller("loginCtrl", function ($scope, $rootScope, $http, $location) {
    $rootScope.oturumacankullanici = null;
    $scope.girisyap = function (mailadresi, sifre) {
        $http.get("http://localhost:1797/api/Kullanici/KullaniciOturumAc?mail=" + mailadresi + "&sifre=" + sifre).then(function (response) {
            if (response.data != null) {
                $rootScope.oturumacankullanici = response.data;
                $location.path("/grup");
            }
            else
                alert("oturum açma işlemi başarısız.");
        });
    }
});
app.controller("grupIslemleriCtrl", function ($scope, $rootScope, $location, $http) {
    $http.get("http://localhost:1797/api/Grup/OturumAcanKullanicininDahilOlduguGruplariGetir?oakid=" + $rootScope.oturumacankullanici.KullaniciID).then(function (response) {
        $scope.gruplar = response.data;
    });

    $scope.grupaltindakimesajlarigor = function (GrupID) {
        $rootScope.grupid = GrupID;
        $location.path("/mesaj");
    }

});
app.controller("mesajIslemleriCtrl", function ($scope, $rootScope, $http, $location) {
    if ($rootScope.grupid == undefined)
        $location.path("/grup")
    $http.get("http://localhost:1797/api/Mesaj/GrupAltindakiMesajlariGetir?gid=" + $rootScope.grupid + "&oakid=" + $rootScope.oturumacankullanici.KullaniciID).then(function (response) {
        $scope.mesajlar = response.data;
    });

    $scope.mesajgonder = function (mesajmetni) {
        $scope.veri = {
            YazanID: $rootScope.oturumacankullanici.KullaniciID,
            GrupID: $rootScope.grupid,
            MesajMetni: mesajmetni
        }

        $http.post("http://localhost:1797/api/Mesaj/YeniMesajEkle", $scope.veri).then(function (response) {
            if (response.data == true) {
                $http.get("http://localhost:1797/api/Mesaj/GrupAltindakiMesajlariGetir?gid=" + $rootScope.grupid + "&oakid=" + $rootScope.oturumacankullanici.KullaniciID).then(function (response) {
                    $scope.mesajlar = response.data;
                });
                $scope.mesajmetni = "";
            }
            else
                alert("mesaj gönderme işleminde hata");
        });
    }

    $scope.mesajisil = function (MesajID) {
        $http.get("http://localhost:1797/api/Mesaj/MesajSil?MesajID=" + MesajID + "&oakid=" + $rootScope.oturumacankullanici.KullaniciID).then(function (response) {
            if (response.data == true) {
                $http.get("http://localhost:1797/api/Mesaj/GrupAltindakiMesajlariGetir?gid=" + $rootScope.grupid + "&oakid=" + $rootScope.oturumacankullanici.KullaniciID).then(function (response) {
                    $scope.mesajlar = response.data;
                });
            }
            else
                alert("mesaj silme işleminde hata");
        });
    }


});
app.controller("indexCtrl", function ($scope, $rootScope, $location) {
    $scope.cikisyap = function () {
        $rootScope.oturumacankullanici = null;
        $location.path("/");
    }
});