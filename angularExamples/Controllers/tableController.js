angular.module('app').controller('tableController', tableController);
tableController.$inject = ['$scope', 'DataProviderService'];
function tableController($scope, DataProviderService) {
  const vm = this;
  vm.people = [];
  vm.currentPerson = {};
  vm.init = init;
  vm.addPerson = addPerson;

  function init() {
    console.log('init');
    vm.people = DataProviderService.getPeople();
  }

  function addPerson() {
    vm.currentPerson.id = vm.people.reduce((previous, current) => {
      return previous.id > current.id ? previous.id : current.id;
    }) + 1;
    vm.people.push(vm.currentPerson);
    vm.currentPerson = {}; // clean object
  }

}