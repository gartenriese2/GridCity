using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridCity.People {
    struct Activity {
        public Date Date { get; set; }
        public Pathfinding.Path Path { get; set; }
    }
}
