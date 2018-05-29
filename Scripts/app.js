var app = angular.module('MyApp', []);
app.controller('MyController', ['$scope', '$http', function ($scope, $http) {
    $scope.SelectedFileForUpload = null;

    $scope.UploadFile = function (files) {
        $scope.$apply(function () { //call this function from File input type control
            $scope.Message = "";
            $scope.SelectedFileForUpload = files[0];
        })
    }

    //Parse excel data
    $scope.ParseExcelDataAndSave = function () {
        var file = $scope.SelectedFileForUpload;
        if (file) {
            var reader = new FileReader();
            reader.onload = function (e) {
                var data = e.target.result;
                //XLSX from js-xlsx library , add in page view page
                var workbook = XLSX.read(data, { type: 'binary' });
                var sheetName = workbook.SheetNames[0];
                var excelData = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
                if (excelData.length > 0) {
                    $scope.SaveData(excelData); //save
                }
                else {
                    $scope.Message = "No data found";
                }
            }
            reader.onerror = function (ex) {
                console.log(ex);
            }

            reader.readAsBinaryString(file);
        }
    }

    //Save excel data to database
    $scope.SaveData = function (excelData) {
        $http({
            method: "POST",
            url: "/Agentii/SaveData",
            data: JSON.stringify(excelData),
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function (data) {
            if (data.status) {
                $scope.Message = excelData.length + " record inserted";
            }
            else {
                $scope.Message = "Failed";
            }
        }, function (error) {
            $scope.Message = "Error";
        })
    }
}])