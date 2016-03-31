using GridCity.Utility.Units;

namespace GridCity {
    interface ITickable {
        bool tick(Time elapsedTime);
    }
}
