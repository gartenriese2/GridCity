namespace GridCity.Scene {

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Fields;
    using Fields.Buildings;
    using People;
    using Utility;

    internal class SceneDescription {

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        public SceneDescription(uint gridWidth, uint gridHeight) {
            Grid = new Grid(gridWidth, gridHeight);
            InitRoads();
            InitBuildings();
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public Grid Grid { get; }

        private FieldFactory Factory { get; } = new FieldFactory();

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public void InitOccupations() {
            var rbs = Grid.GetFields<ResidentialBuilding>();
            var wbs = Grid.GetFields<OccupationalBuilding>().Where(x => x.HasOpenOccupations(Resident.Type.WORKER)).ToList();
            var sbs = Grid.GetFields<OccupationalBuilding>().Where(x => x.HasOpenOccupations(Resident.Type.TEEN)).ToList();
            var ubs = Grid.GetFields<OccupationalBuilding>().Where(x => x.HasOpenOccupations(Resident.Type.STUDENT)).ToList();

            List<Resident> totalResidents = new List<Resident>();
            foreach (var rb in rbs) {
                totalResidents.AddRange(rb.Residents);
            }

            int numCores = Environment.ProcessorCount;
            Debug.Assert(numCores > 0, "there should be at least 1 core!");
            if (numCores == 1) {
                FindOccupations(totalResidents, wbs, sbs, ubs);
            } else {
                int residentsPerCore = totalResidents.Count / numCores;
                int residentsForLastCore = totalResidents.Count - (residentsPerCore * (numCores - 1));
                List<Task> tasks = new List<Task>();
                for (int i = 0; i < numCores - 1; ++i) {
                    tasks.Add(FindOccupationsAsync(totalResidents.GetRange(i * residentsPerCore, residentsPerCore), wbs, sbs, ubs));
                }

                tasks.Add(FindOccupationsAsync(totalResidents.GetRange((numCores - 1) * residentsPerCore, residentsForLastCore), wbs, sbs, ubs));
                Task allTasks = Task.WhenAll(tasks.ToArray());
                FindOccupationsAsync(allTasks);
                allTasks.Wait();
            }
        }

        public void PrintResidents() {
            uint pensioners = 0;
            uint workers = 0;
            uint unemployed = 0;
            uint infants = 0;
            uint kids = 0;
            uint teens = 0;
            uint students = 0;
            var rbs = Grid.GetFields<ResidentialBuilding>();
            foreach (var rb in rbs) {
                foreach (var hh in rb.Households) {
                    foreach (var res in hh.Residents) {
                        if (res is Pensioner) {
                            ++pensioners;
                        }

                        if (res is Worker) {
                            ++workers;
                        }

                        if (res is Unemployed) {
                            ++unemployed;
                        }

                        if (res is Infant) {
                            ++infants;
                        }

                        if (res is Kid) {
                            ++kids;
                        }

                        if (res is Teen) {
                            ++teens;
                        }

                        if (res is Student) {
                            ++students;
                        }
                    }
                }
            }

            Console.Write("There are {0} pensioners, {1} workers, {2} unemployed, {3} infants, {4} kids, {5} teens, and {6} students.\n", pensioners, workers, unemployed, infants, kids, teens, students);
        }

        private void InitRoads() {
            List<ConnectableField> street0 = new List<ConnectableField>();
            street0.Add(Grid.SetField(Factory.GetRoad("EndRoad", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 18u))));
            street0.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 17u))));
            street0.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 16u))));
            street0.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 15u))));
            street0.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 14u))));
            street0.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 13u))));
            street0.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 12u))));
            street0.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 11u))));
            street0.Add(Grid.SetField(Factory.GetRoad("TCrossingWithCrosswalks", ConnectableField.Orientation_CW.TWOSEVENTY, new GlobalCoordinate(2u, 10u))));
            street0.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 9u))));
            street0.Add(Grid.SetField(Factory.GetRoad("StraightRoad", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 8u))));
            street0.Add(Grid.SetField(Factory.GetRoad("StraightRoad", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 7u))));
            street0.Add(Grid.SetField(Factory.GetRoad("TCrossingWithCrosswalks", ConnectableField.Orientation_CW.TWOSEVENTY, new GlobalCoordinate(2u, 6u))));
            street0.Add(Grid.SetField(Factory.GetRoad("StraightRoad", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 5u))));
            street0.Add(Grid.SetField(Factory.GetRoad("StraightRoad", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 4u))));
            street0.Add(Grid.SetField(Factory.GetRoad("StraightRoad", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 3u))));
            street0.Add(Grid.SetField(Factory.GetRoad("EndRoad", ConnectableField.Orientation_CW.ONEEIGHTY, new GlobalCoordinate(2u, 2u))));
            ConnectableField.Connect(street0);

            List<ConnectableField> street1 = new List<ConnectableField>();
            street1.Add(Grid.SetField(Factory.GetRoad("EndRoad", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 18u))));
            street1.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 17u))));
            street1.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 16u))));
            street1.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 15u))));
            street1.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 14u))));
            street1.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 13u))));
            street1.Add(Grid.SetField(Factory.GetRoad("TCrossingWithCrosswalks", ConnectableField.Orientation_CW.TWOSEVENTY, new GlobalCoordinate(10u, 12u))));
            street1.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 11u))));
            street1.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 10u))));
            street1.Add(Grid.SetField(Factory.GetRoad("TCrossingWithCrosswalks", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(10u, 9u))));
            street1.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 8u))));
            street1.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 7u))));
            street1.Add(Grid.SetField(Factory.GetRoad("TCrossingWithCrosswalks", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(10u, 6u))));
            street1.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 5u))));
            street1.Add(Grid.SetField(Factory.GetRoad("EndRoad", ConnectableField.Orientation_CW.ONEEIGHTY, new GlobalCoordinate(10u, 4u))));
            ConnectableField.Connect(street1);

            List<ConnectableField> street2 = new List<ConnectableField>();
            street2.Add(street0[12]);
            street2.Add(Grid.SetField(Factory.GetRoad("StraightRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(3u, 6u))));
            street2.Add(Grid.SetField(Factory.GetRoad("StraightRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(4u, 6u))));
            street2.Add(Grid.SetField(Factory.GetRoad("StraightRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(5u, 6u))));
            street2.Add(Grid.SetField(Factory.GetRoad("StraightRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(6u, 6u))));
            street2.Add(Grid.SetField(Factory.GetRoad("StraightRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(7u, 6u))));
            street2.Add(Grid.SetField(Factory.GetRoad("StraightRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(8u, 6u))));
            street2.Add(Grid.SetField(Factory.GetRoad("StraightRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(9u, 6u))));
            street2.Add(street1[12]);
            ConnectableField.Connect(street2);

            List<ConnectableField> street3 = new List<ConnectableField>();
            street3.Add(street0[8]);
            street3.Add(Grid.SetField(Factory.GetRoad("StraightRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(3u, 10u))));
            street3.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(4u, 10u))));
            street3.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(5u, 10u))));
            street3.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(6u, 10u))));
            street3.Add(Grid.SetField(Factory.GetRoad("Curve", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(7u, 10u))));
            street3.Add(Grid.SetField(Factory.GetRoad("Curve", ConnectableField.Orientation_CW.ONEEIGHTY, new GlobalCoordinate(7u, 9u))));
            street3.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(8u, 9u))));
            street3.Add(Grid.SetField(Factory.GetRoad("StraightRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(9u, 9u))));
            street3.Add(street1[9]);
            ConnectableField.Connect(street3);

            List<ConnectableField> street4 = new List<ConnectableField>();
            street4.Add(Grid.GetField<ConnectableField>(new GlobalCoordinate(10, 12)));
            street4.Add(Grid.SetField(Factory.GetRoad("StraightRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(11, 12))));
            street4.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(12, 12))));
            street4.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(13, 12))));
            street4.Add(Grid.SetField(Factory.GetRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(14, 12))));
            street4.Add(Grid.SetField(Factory.GetRoad("EndRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(15, 12))));
            ConnectableField.Connect(street4);
        }

        private bool ConnectBuilding(ConnectableField bld, ConnectableField.Orientation_CW orientation, uint x, uint y) {
            switch (orientation) {
                case ConnectableField.Orientation_CW.NINETY:
                    return bld.ConnectTo(Grid.GetField<ConnectableField>(new GlobalCoordinate(x - 1, y)));
                case ConnectableField.Orientation_CW.ONEEIGHTY:
                    return bld.ConnectTo(Grid.GetField<ConnectableField>(new GlobalCoordinate(x, y + 1)));
                case ConnectableField.Orientation_CW.TWOSEVENTY:
                    return bld.ConnectTo(Grid.GetField<ConnectableField>(new GlobalCoordinate(x + 1, y)));
                case ConnectableField.Orientation_CW.ZERO:
                    return bld.ConnectTo(Grid.GetField<ConnectableField>(new GlobalCoordinate(x, y - 1)));
                default:
                    throw new ArgumentOutOfRangeException("orientation", "Unknown enum");
            }
        }

        private void AddResidentialBuilding(string name, ConnectableField.Orientation_CW orientation, uint x, uint y) {
            var bld = (ConnectableField)Grid.SetField(Factory.GetResidentialBuilding(name, orientation, new GlobalCoordinate(x, y)));
            bool success = ConnectBuilding(bld, orientation, x, y);
            Debug.Assert(success, "Building at (" + x + "|" + y + ") could not connect with the following orientation: " + orientation.ToString());
        }

        private void AddOccupationalBuilding<T>(string name, ConnectableField.Orientation_CW orientation, uint x, uint y) where T : OccupationalBuilding {
            var bld = (ConnectableField)Grid.SetField(Factory.GetOccupationalBuilding<T>(name, orientation, new GlobalCoordinate(x, y)));
            bool success = ConnectBuilding(bld, orientation, x, y);
            Debug.Assert(success, "Building at (" + x + "|" + y + ") could not connect with the following orientation: " + orientation.ToString());
        }

        private void InitBuildings() {
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 1, 17);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 1, 16);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 1, 15);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 1, 14);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 1, 13);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 1, 12);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 3, 17);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 3, 16);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 3, 15);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 3, 14);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 3, 13);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 3, 12);

            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 9, 17);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 9, 16);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 9, 15);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 9, 14);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 9, 13);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 11, 17);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 11, 16);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 11, 15);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 11, 14);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 11, 13);

            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.ZERO, 12, 13);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.ZERO, 13, 13);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.ZERO, 14, 13);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.ONEEIGHTY, 12, 11);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.ONEEIGHTY, 13, 11);
            AddResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.ONEEIGHTY, 14, 11);

            AddResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 9, 11);
            AddResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 11, 11);
            AddResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 9, 10);
            AddResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 11, 10);

            AddResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.ONEEIGHTY, 8, 8);
            AddResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.ZERO, 8, 10);

            AddResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 11, 5);
            AddOccupationalBuilding<WorkBuilding>("WorkBuilding", ConnectableField.Orientation_CW.NINETY, 3, 11);
            AddResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 1, 11);
            AddResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.ZERO, 4, 11);
            AddResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.ZERO, 5, 11);
            AddResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.ZERO, 6, 11);
            AddOccupationalBuilding<WorkBuilding>("WorkBuilding", ConnectableField.Orientation_CW.NINETY, 3, 9);
            AddResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.ONEEIGHTY, 4, 9);
            AddOccupationalBuilding<WorkBuilding>("WorkBuilding", ConnectableField.Orientation_CW.ONEEIGHTY, 5, 9);
            AddOccupationalBuilding<WorkBuilding>("WorkBuilding", ConnectableField.Orientation_CW.ONEEIGHTY, 6, 9);
            AddOccupationalBuilding<WorkBuilding>("WorkBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 1, 9);
            AddOccupationalBuilding<School>("SchoolBuilding", ConnectableField.Orientation_CW.NINETY, 11, 7);
            AddOccupationalBuilding<University>("UniversityBuilding", ConnectableField.Orientation_CW.NINETY, 11, 8);
            AddOccupationalBuilding<WorkBuilding>("WorkBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 9, 7);
            AddResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 9, 8);
        }

        private async void FindOccupationsAsync(Task task) {
            await task;
        }

        private bool FindOccupation<T>(T resident, List<OccupationalBuilding> obs) where T : Occupant {
            int loopCounter = 0;
            bool foundOccupation = resident.FindOccupation(obs);
            while (!foundOccupation && loopCounter < 1000) { // TODO: get rid of magic number, e.g. use stopwatch
                ++loopCounter;
                foundOccupation = resident.FindOccupation(obs);
            }

            return foundOccupation;
        }

        private void FindOccupations(List<Resident> residents, List<OccupationalBuilding> wbs, List<OccupationalBuilding> sbs, List<OccupationalBuilding> ubs) {
            foreach (var resident in residents) {
                if (resident is Worker) {
                    if (!FindOccupation(resident as Worker, wbs)) {
                        throw new Exception("Worker did not find a job"); // TODO: prevent from occasionally throwing
                    }
                } else if (resident is Teen) {
                    if (!FindOccupation(resident as Teen, sbs)) {
                        throw new Exception("Teen did not find a school");
                    }
                } else if (resident is Student) {
                    if (!FindOccupation(resident as Student, ubs)) {
                        throw new Exception("Student did not find a university");
                    }
                }
            }

            ////Console.WriteLine("This thread is done with finding occupations!");
        }

        private Task FindOccupationsAsync(List<Resident> residents, List<OccupationalBuilding> wbs, List<OccupationalBuilding> sbs, List<OccupationalBuilding> ubs) {
            return Task.Run(() => FindOccupations(residents, wbs, sbs, ubs));
        }
    }
}
