angular.module('app')
  .factory('DataProviderService', DataProviderService);
DataProviderService.$inject = [];
function DataProviderService() {
  var service = {
    getPeople: getPeople,
    getCountries: getCountries,
    getCurrencies: getCurrencies
  };
  return service;

  function getPeople() {
    return [
      {id: 1, name: 'juan', lastname: 'Nieves', money: 300},
      {id: 2, name: 'Juan', lastname: 'Martinez', money: 300},
      {id: 3, name: 'Martin', lastname: 'Roldan', money: 300},
      {id: 4, name: 'Mario', lastname: 'Franco', money: 300},
      {id: 5, name: 'Rodrigo', lastname: 'Borja', money: 300},
    ];
  }

  function getCountries() {
    return [
      {id: 1, name: 'El Salvador'},
      {id: 2, name: 'Colombia'},
      {id: 3, name: 'USA'},
      {id: 4, name: 'Canada'}
    ];
  }

  function getCurrencies() {
    return [
      {id: 1, moneda: 'Colon'},
      {id: 2, moneda: 'Balboa'},
      {id: 3, moneda: 'Dolar'},
      {id: 4, moneda: 'Cacao'}
    ];
  }
}