angular
    .module('app')
    .controller('modalCtrl', modalCtrl)

function modalCtrl($scope, $uibModal, cfg, apiRoot) {

    $scope.openw = function () {

        var modalInstance = $uibModal.open({
            templateUrl: apiRoot + "Wizard/Inicio/ShowWiz",
            size: "lg",
            backdrop: "static",
            keyboard: false,
            controller: wizardOneCtrl,
            windowClass: 'col-lg-12 col-md-12 col-xs-12'
        });
    };

};

//function wizardOneCtrl($scope, notify, sweetAlert) {
function wizardOneCtrl($scope, $uibModalInstance, cfg, apiRoot, $http, $window, $sce) {

    $scope.savewizardform = apiRoot + "Wizard/Inicio/Savewizard";
    $scope.Getinfoclienturl = apiRoot + "Wizard/Inicio/GetInfoClient";
    $scope.checkifuserexist = apiRoot + "Wizard/Inicio/Ifuserexist";
    $scope.checkifemailexist = apiRoot + "Wizard/Inicio/Ifemailexist";
    //$scope.selectedImage= $sce.trustAsHtml("<img src='~/Content/img/sqrelyLog.png' class='img-responsive'/>");
    $scope.title = $sce.trustAsHtml("Wizard");
    $scope.infoplan = "";
    $scope.username = "";
    $scope.userlastname = "";
    $scope.alias = "";
    $scope.password = "";
    $scope.email = "";
    $scope.file = "Default";
    $scope.allowusers = 0;
    $scope.userindex = "";
    $scope.allroles = "";
    $scope.allregion = "";
    $scope.allLanguage = "";
    $scope.allcurrency = "";
    $scope.allTimezone = "";
    $scope.selectedRol = "";
    $scope.SelectedCurency = "";
    $scope.samebilladd = false;
    $scope.userexist = false;
    $scope.actuallyusers = [];
    $scope.config = {
        usuarios: [],
        tenantconfigs: [],
        settings: [],
        numberusers: 0
    }

    $scope.InfoClient = function () {

        $http.get($scope.Getinfoclienturl).then(function (response) {
            console.log(response.data.defaultconfigs);
            console.log(response.data.settdefaultconfigs);
            //$scope.infoplan = response;
            $scope.config.tenantconfigs = response.data.tenantconfigs;
            $scope.config.settings = response.data.settconfigs;

            $scope.allcurrency = response.data.currencies;
            $scope.allLanguage = response.data.languanges;
            $scope.allregion = response.data.regions;
            $scope.allTimezone = response.data.Timezones;
            $scope.allowusers = response.data.AllocatedUsers - 1;
            //$scope.actuallyusers = response.userscount;
            //$scope.flagskip = response.defaultconfigs.length;
            if (response.data.paid == true) {
                $scope.config.tenantconfigs[0].parameter = response.data.Companyname;
                $scope.config.tenantconfigs[2].parameter = "default";
                $scope.config.tenantconfigs[3].parameter = response.data.address;
                $scope.config.tenantconfigs[5].parameter = response.data.owneremail;
                $scope.config.tenantconfigs[6].parameter = response.data.phone;
                $scope.config.tenantconfigs[8].parameter = response.data.ownerurl;
                $scope.config.tenantconfigs[9].parameter = true;
            }

            if (response.data.defaultconfigs.length > 1) {
                $scope.config.tenantconfigs[0].parameter = response.data.defaultconfigs[0].parameter;
                $scope.config.tenantconfigs[1].parameter = response.data.defaultconfigs[1].parameter;
                $scope.config.tenantconfigs[2].parameter = response.data.defaultconfigs[2].parameter;
                $scope.config.tenantconfigs[3].parameter = response.data.defaultconfigs[3].parameter;
                $scope.config.tenantconfigs[4].parameter = response.data.defaultconfigs[4].parameter;
                $scope.config.tenantconfigs[5].parameter = response.data.defaultconfigs[5].parameter;
                $scope.config.tenantconfigs[6].parameter = response.data.defaultconfigs[6].parameter;
                $scope.config.tenantconfigs[7].parameter = response.data.defaultconfigs[7].parameter;
                $scope.config.tenantconfigs[8].parameter = response.data.defaultconfigs[8].parameter;
                $scope.config.tenantconfigs[9].parameter = true;
                if (response.data.defaultconfigs[2].parameter != "default") {
                    $scope.selectedImage = "data:image/png;base64," + response.data.defaultconfigs[2].parameter;
                }
                else {
                    $scope.config.tenantconfigs[2].parameter = "default";
                }
                $scope.setRegion(response.data.settdefaultconfigs[0].parameter);
                $scope.setLanguage(response.data.settdefaultconfigs[1].parameter);
                $scope.settimezone(response.data.settdefaultconfigs[2].parameter);
                $scope.setcurrency(response.data.settdefaultconfigs[3].parameter);
                $scope.SelectedRegion = $scope.allregion[$scope.idregion];
                $scope.SelectedLanguage = $scope.allLanguage[$scope.idlang];
                $scope.SelectedTimezone = $scope.allTimezone[$scope.idtimez];
                $scope.SelectedCurency = $scope.allcurrency[$scope.idcurr];
                $scope.AsigCurrency();
                $scope.AsigTimezone();
                $scope.AsigLanguage();
                $scope.AsigRegion();

            }
            else {
                $scope.config.tenantconfigs[2].parameter = "default";
                $scope.config.tenantconfigs[9].parameter = true;
            }
            $scope.allroles = response.data.userroles;
            //$scope.config.usuarios = response.userscount
            console.log("*************TENANT***********");
            //console.log($scope.infoplan);

                       
        });
        
    }

    $scope.setRegion = function (region) {
        for (i = 0; i < $scope.allregion.length; i++) {
            if ($scope.allregion[i].name == region) {
                $scope.idregion = i;
            }
        }
    }
    $scope.setLanguage = function (lang) {
        for (i = 0; i < $scope.allLanguage.length; i++) {
            if ($scope.allLanguage[i].name == lang) {
                $scope.idlang = i;
            }
        }
    }
    $scope.settimezone = function (timez) {
        for (i = 0; i < $scope.allTimezone.length; i++) {
            if ($scope.allTimezone[i].timeZone == timez) {
                $scope.idtimez = i;
            }
        }
    }
    $scope.setcurrency = function (curr) {
        for (i = 0; i < $scope.allcurrency.length; i++) {
            if ($scope.allcurrency[i].code == curr) {
                $scope.idcurr = i;
            }
        }
    }

    $scope.Adduser = function () {

        $scope.config.usuarios.push({
            username: $scope.username,
            userlastname: $scope.userlastname,
            alias: $scope.alias,
            password: $scope.password,
            email: $scope.email,
            rol: $scope.selectedRol
        });
        $scope.allowusers += 1;
        $scope.config.numberusers = $scope.config.usuarios.length;
        $scope.isupdatevalid = false;
        $scope.userexist = false;

        $scope.username = "";
        $scope.userlastname ="";
        $scope.alias = "";
        $scope.password = "";
        $scope.email = "";
        $scope.selectedRol = "";

    };

    $scope.remove = function (item) {
        $scope.config.usuarios.splice(item, 1);
        $scope.allowusers -= 1;
    }

    $scope.edit = function (item) {

        $scope.username = $scope.config.usuarios[item].username;
        $scope.userlastname = $scope.config.usuarios[item].userlastname;
        $scope.alias = $scope.config.usuarios[item].alias;
        $scope.password = $scope.config.usuarios[item].password;
        $scope.email = $scope.config.usuarios[item].email;
        $scope.selectedRol = $scope.config.usuarios[item].rol;
        $scope.isupdatevalid = true;
        $scope.userindex = item;
        $scope.userexist = true;
        $scope.emailexist = true;
    }
    $scope.SameAdd = function () {
        if ($scope.samebilladd == true) {
            $scope.config.tenantconfigs[4].parameter = $scope.config.tenantconfigs[3].parameter;
        }
        else {
            $scope.config.tenantconfigs[4].parameter = "";
        }


    }
    $scope.Updateuser = function (item) {

        if ($scope.config.usuarios[item] != null) {

            $scope.config.usuarios[item].username = $scope.username;
            $scope.config.usuarios[item].userlastname = $scope.userlastname;
            $scope.config.usuarios[item].alias = $scope.alias;
            $scope.config.usuarios[item].password = $scope.password;
            $scope.config.usuarios[item].email = $scope.email;
            $scope.config.usuarios[item].rol = $scope.selectedRol;

            $scope.isupdatevalid = false;
        }
        else {
            //Show notification
            swal({
                title: "Error",
                text: "No puedes igresar mas usuarios",
                type: "error"
            });
            $scope.isupdatevalid = false;
        }
        $scope.username = "";
        $scope.userlastname="";
        $scope.alias = "";
        $scope.password = "";
        $scope.email = "";
        $scope.selectedRol = "";
        $scope.userexist = false;
        $scope.emailexist = false;
    }

    //$scope.ifuserexist = function () {
    //    $scope.userexist = false;
    //    if ($scope.alias != "" && $scope.alias.length > 5) {
    //        if($scope.config.usuarios.length > 0){
    //            for (var i = 0; i < $scope.config.usuarios.length; i++) {
    //                if ($scope.alias == $scope.config.usuarios[i].alias) {
    //                    $scope.userexist = true;
    //                }
    //            }
    //        }
    //        if($scope.actuallyusers.length > 0){
    //            for (var j = 0; j < $scope.actuallyusers.length; j++) {
    //                if ($scope.alias == $scope.actuallyusers[j].alias || $scope.alias == $scope.config.usuarios[j].alias) {
    //                    $scope.userexist = true;
    //                }
    //            }
    //        }
    //    }
    //}
    $scope.ifuserexist = function () {
        $scope.userexist = false;
        if ($scope.alias != "" && $scope.alias.length > 5) {
            if ($scope.config.usuarios.length > 0) {
                for (var i = 0; i < $scope.config.usuarios.length; i++) {
                    if ($scope.alias == $scope.config.usuarios[i].alias) {
                        $scope.userexist = true;
                    }

                }
            }
            $http.get($scope.checkifuserexist, {
                params: { username : $scope.alias}
            }).then(function (response) {
                if (response.data == 1) { $scope.userexist = true; }
            }, function (response) {
                $scope.userexist = true;
            })
        }

    }
    $scope.Ifemailexist = function (){
        $scope.emailexist = false;
        if ($scope.email != "" && $scope.email.indexOf("@") > -1) {
            if ($scope.config.usuarios.length > 0) {
                for (var i = 0; i < $scope.config.usuarios.length; i++) {
                    if ($scope.email == $scope.config.usuarios[i].email) {
                        $scope.emailexist = true;
                    }

                }
            }
            $http.get($scope.checkifemailexist, {
                params: { email: $scope.email }
            }).then(function (response) {
                if (response.data == 1) { $scope.emailexist = true; }
            }, function (response) {
                $scope.emailexist = true;
            })
        }
        
    };

    $scope.AsigCurrency = function () {
        $scope.config.settings[5].parameter = $scope.SelectedCurency.code;
    }
    $scope.AsigTimezone = function () {
        $scope.config.settings[4].parameter = $scope.SelectedTimezone.timeZone;
    }
    $scope.AsigLanguage = function () {
        $scope.config.settings[3].parameter = $scope.SelectedLanguage.name;
    }
    $scope.AsigRegion = function () {
        $scope.config.settings[2].parameter = $scope.SelectedRegion.name;
    }
    $scope.cargarimage = function () {
        if ($scope.file != null) {
            $scope.config.tenantconfigs[2].parameter = $scope.file.base64;
            $scope.selectedImage = "data:image/png;base64," + $scope.file.base64;
        }

    };

    $scope.intervalFunction = function () {
        $timeout(function () {
            //$scope.getData();
            //$scope.intervalFunction();
            $scope.selectedImage = "data:image/png;base64," + $scope.file.base64;
        }, 1500)
    };

    $scope.EndWizard = function () {
        $window.location.href = apiRoot + "Dashboard/Dashboard"
    }

    // Initial step
    $scope.step = 1;

    // Wizard functions
    $scope.wizard = {
        show: function (number) {
            $scope.step = number;
        },
        next: function () {
            $scope.step++;
        },
        prev: function () {
            $scope.step--;
        }
    };

    $scope.submit = function () {
        //$scope.step = 1;
        $scope.savewizard();
    }

    $scope.finishmodal = function () {
        $modalInstance.close();
    }

    $scope.savewizard = function () {
        console.log("Wizzard");
        console.log($scope.config);
        $http({
            url: $scope.savewizardform,
            dataType: 'json',
            method: 'POST',
            data: $scope.config,
            headers: {
                "Content-Type": "application/json"
            }
        }).success(
            function (response) {
                $scope.status = response;
                console.log(response);
                console.log("**** variable");
                console.log($scope.status);

                if ($scope.status.status.code == 1) {
                    
                    $uibModalInstance.close();
                };

            });

    };

}