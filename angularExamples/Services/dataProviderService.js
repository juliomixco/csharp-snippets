angular.module('app')
  .factory('DataProviderService', DataProviderService);
DataProviderService.$inject = [];
function DataProviderService() {
  var service = {
    getPeople: getPeople
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
}