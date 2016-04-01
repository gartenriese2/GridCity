﻿using GridCity.Fields;
using GridCity.Fields.Buildings;
using System.Collections.Generic;
using System;
using GridCity.People;
using GridCity.Utility;
using System.Diagnostics;
using System.Linq;

namespace GridCity {
    class Scene {
        private Grid Grid { get; }
        private FieldFactory Factory { get; } = new FieldFactory();
        public Scene(Grid grid) {
            Grid = grid;
            initRoads();
            initBuildings();
            initOccupations();
        }
        private void initRoads() {
            List<ConnectableField> street0 = new List<ConnectableField>();
            street0.Add(Grid.setField(Factory.getRoad("EndRoad", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 18u))));
            street0.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 17u))));
            street0.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 16u))));
            street0.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 15u))));
            street0.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 14u))));
            street0.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 13u))));
            street0.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 12u))));
            street0.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 11u))));
            street0.Add(Grid.setField(Factory.getRoad("TCrossingWithCrosswalks", ConnectableField.Orientation_CW.TWOSEVENTY, new GlobalCoordinate(2u, 10u))));
            street0.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 9u))));
            street0.Add(Grid.setField(Factory.getRoad("StraightRoad", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 8u))));
            street0.Add(Grid.setField(Factory.getRoad("StraightRoad", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 7u))));
            street0.Add(Grid.setField(Factory.getRoad("TCrossingWithCrosswalks", ConnectableField.Orientation_CW.TWOSEVENTY, new GlobalCoordinate(2u, 6u))));
            street0.Add(Grid.setField(Factory.getRoad("StraightRoad", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 5u))));
            street0.Add(Grid.setField(Factory.getRoad("StraightRoad", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 4u))));
            street0.Add(Grid.setField(Factory.getRoad("StraightRoad", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(2u, 3u))));
            street0.Add(Grid.setField(Factory.getRoad("EndRoad", ConnectableField.Orientation_CW.ONEEIGHTY, new GlobalCoordinate(2u, 2u))));
            ConnectableField.connect(street0);

            List<ConnectableField> street1 = new List<ConnectableField>();
            street1.Add(Grid.setField(Factory.getRoad("EndRoad", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 18u))));
            street1.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 17u))));
            street1.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 16u))));
            street1.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 15u))));
            street1.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 14u))));
            street1.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 13u))));
            street1.Add(Grid.setField(Factory.getRoad("TCrossingWithCrosswalks", ConnectableField.Orientation_CW.TWOSEVENTY, new GlobalCoordinate(10u, 12u))));
            street1.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 11u))));
            street1.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 10u))));
            street1.Add(Grid.setField(Factory.getRoad("TCrossingWithCrosswalks", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(10u, 9u))));
            street1.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 8u))));
            street1.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 7u))));
            street1.Add(Grid.setField(Factory.getRoad("TCrossingWithCrosswalks", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(10u, 6u))));
            street1.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(10u, 5u))));
            street1.Add(Grid.setField(Factory.getRoad("EndRoad", ConnectableField.Orientation_CW.ONEEIGHTY, new GlobalCoordinate(10u, 4u))));
            ConnectableField.connect(street1);

            List<ConnectableField> street2 = new List<ConnectableField>();
            street2.Add(street0[12]);
            street2.Add(Grid.setField(Factory.getRoad("StraightRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(3u, 6u))));
            street2.Add(Grid.setField(Factory.getRoad("StraightRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(4u, 6u))));
            street2.Add(Grid.setField(Factory.getRoad("StraightRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(5u, 6u))));
            street2.Add(Grid.setField(Factory.getRoad("StraightRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(6u, 6u))));
            street2.Add(Grid.setField(Factory.getRoad("StraightRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(7u, 6u))));
            street2.Add(Grid.setField(Factory.getRoad("StraightRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(8u, 6u))));
            street2.Add(Grid.setField(Factory.getRoad("StraightRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(9u, 6u))));
            street2.Add(street1[12]);
            ConnectableField.connect(street2);

            List<ConnectableField> street3 = new List<ConnectableField>();
            street3.Add(street0[8]);
            street3.Add(Grid.setField(Factory.getRoad("StraightRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(3u, 10u))));
            street3.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(4u, 10u))));
            street3.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(5u, 10u))));
            street3.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(6u, 10u))));
            street3.Add(Grid.setField(Factory.getRoad("Curve", ConnectableField.Orientation_CW.ZERO, new GlobalCoordinate(7u, 10u))));
            street3.Add(Grid.setField(Factory.getRoad("Curve", ConnectableField.Orientation_CW.ONEEIGHTY, new GlobalCoordinate(7u, 9u))));
            street3.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(8u, 9u))));
            street3.Add(Grid.setField(Factory.getRoad("StraightRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(9u, 9u))));
            street3.Add(street1[9]);
            ConnectableField.connect(street3);

            List<ConnectableField> street4 = new List<ConnectableField>();
            street4.Add(Grid.getField<ConnectableField>(new GlobalCoordinate(10, 12)));
            street4.Add(Grid.setField(Factory.getRoad("StraightRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(11, 12))));
            street4.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(12, 12))));
            street4.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(13, 12))));
            street4.Add(Grid.setField(Factory.getRoad("StraightRoadWithBuildingAccess", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(14, 12))));
            street4.Add(Grid.setField(Factory.getRoad("EndRoad", ConnectableField.Orientation_CW.NINETY, new GlobalCoordinate(15, 12))));
            ConnectableField.connect(street4);
        }
        private bool connectBuilding(ConnectableField bld, ConnectableField.Orientation_CW orientation, uint x, uint y) {
            switch (orientation) {
                case ConnectableField.Orientation_CW.NINETY:
                    return bld.connectTo(Grid.getField<ConnectableField>(new GlobalCoordinate(x - 1, y)));
                case ConnectableField.Orientation_CW.ONEEIGHTY:
                    return bld.connectTo(Grid.getField<ConnectableField>(new GlobalCoordinate(x, y + 1)));
                case ConnectableField.Orientation_CW.TWOSEVENTY:
                    return bld.connectTo(Grid.getField<ConnectableField>(new GlobalCoordinate(x + 1, y)));
                case ConnectableField.Orientation_CW.ZERO:
                    return bld.connectTo(Grid.getField<ConnectableField>(new GlobalCoordinate(x, y - 1)));
                default:
                    throw new ArgumentOutOfRangeException("orientation", "Unknown enum");
            }
        }
        private void addResidentialBuilding(string name, ConnectableField.Orientation_CW orientation, uint x, uint y) {
            var bld = (ConnectableField)Grid.setField(Factory.getResidentialBuilding(name, orientation, new GlobalCoordinate(x, y)));
            bool success = connectBuilding(bld, orientation, x, y);
            Debug.Assert(success, "Building at (" + x + "|" + y + ") could not connect with the following orientation: " + orientation.ToString());
        }
        private void addOccupationalBuilding<T>(string name, ConnectableField.Orientation_CW orientation, uint x, uint y) where T : OccupationalBuilding {
            var bld = (ConnectableField)Grid.setField(Factory.getOccupationalBuilding<T>(name, orientation, new GlobalCoordinate(x, y)));
            bool success = connectBuilding(bld, orientation, x, y);
            Debug.Assert(success, "Building at (" + x + "|" + y + ") could not connect with the following orientation: " + orientation.ToString());
        }
        private void initBuildings() {
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 1, 17);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 1, 16);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 1, 15);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 1, 14);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 1, 13);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 1, 12);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 3, 17);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 3, 16);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 3, 15);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 3, 14);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 3, 13);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 3, 12);

            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 9, 17);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 9, 16);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 9, 15);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 9, 14);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 9, 13);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 11, 17);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 11, 16);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 11, 15);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 11, 14);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 11, 13);

            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.ZERO, 12, 13);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.ZERO, 13, 13);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.ZERO, 14, 13);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.ONEEIGHTY, 12, 11);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.ONEEIGHTY, 13, 11);
            addResidentialBuilding("SmallResidentialBuilding", ConnectableField.Orientation_CW.ONEEIGHTY, 14, 11);

            addResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 9, 11);
            addResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 11, 11);
            addResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 9, 10);
            addResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 11, 10);

            addResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.ONEEIGHTY, 8, 8);
            addResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.ZERO, 8, 10);

            addResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.NINETY, 11, 5);
            addOccupationalBuilding<WorkBuilding>("WorkBuilding", ConnectableField.Orientation_CW.NINETY, 3, 11);
            addResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 1, 11);
            addResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.ZERO, 4, 11);
            addResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.ZERO, 5, 11);
            addResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.ZERO, 6, 11);
            addOccupationalBuilding<WorkBuilding>("WorkBuilding", ConnectableField.Orientation_CW.NINETY, 3, 9);
            addResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.ONEEIGHTY, 4, 9);
            addOccupationalBuilding<WorkBuilding>("WorkBuilding", ConnectableField.Orientation_CW.ONEEIGHTY, 5, 9);
            addOccupationalBuilding<WorkBuilding>("WorkBuilding", ConnectableField.Orientation_CW.ONEEIGHTY, 6, 9);
            addOccupationalBuilding<WorkBuilding>("WorkBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 1, 9);
            addOccupationalBuilding<School>("SchoolBuilding", ConnectableField.Orientation_CW.NINETY, 11, 7);
            addOccupationalBuilding<University>("UniversityBuilding", ConnectableField.Orientation_CW.NINETY, 11, 8);
            addOccupationalBuilding<WorkBuilding>("WorkBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 9, 7);
            addResidentialBuilding("MediumResidentialBuilding", ConnectableField.Orientation_CW.TWOSEVENTY, 9, 8);
        }
        private void initOccupations() {
            var rbs = Grid.getFields<ResidentialBuilding>();
            var wbs = Grid.getFields<OccupationalBuilding>().Where(x => x.HasOpenOccupations(Resident.Type.WORKER)).ToList();
            var sbs = Grid.getFields<OccupationalBuilding>().Where(x => x.HasOpenOccupations(Resident.Type.TEEN)).ToList();
            var ubs = Grid.getFields<OccupationalBuilding>().Where(x => x.HasOpenOccupations(Resident.Type.STUDENT)).ToList();
            foreach (var rb in rbs) {
                foreach (var household in rb.Households) {
                    foreach (var resident in household.Residents) {
                        if (resident is Worker) {
                            if (!((Worker)resident).findOccupation(wbs)) {
                                throw new NotSupportedException("All workers need to find jobs!");
                            }
                        } else if (resident is Teen) {
                            if (!((Teen)resident).findOccupation(sbs)) {
                                throw new NotSupportedException("All teens need to find a school!");
                            }
                        } else if (resident is Student) {
                            if (!((Student)resident).findOccupation(ubs)) {
                                throw new NotSupportedException("All students need to find a university!");
                            }
                        }
                    }
                }
            }
        }
        public void printResidents() {
            uint pensioners = 0;
            uint workers = 0;
            uint unemployed = 0;
            uint infants = 0;
            uint kids = 0;
            uint teens = 0;
            uint students = 0;
            var rbs = Grid.getFields<ResidentialBuilding>();
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
    }
}
