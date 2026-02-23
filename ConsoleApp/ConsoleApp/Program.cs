
using ConsoleApp.LSP;

var car = new Car();
var motorcycle = new Motorcycle();

var operator1 = new VehicleOperator();
operator1.OperateVehicle(car);          // Outputs car-specific messages
operator1.OperateVehicle(motorcycle);    // Outputs motorcycle-specific messages