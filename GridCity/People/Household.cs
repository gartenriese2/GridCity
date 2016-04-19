namespace GridCity.People {

    using System;
    using System.Collections.Generic;
    using Fields.Buildings;

    internal class Household {

        //---------------------------------------------------------------------
        // Constructors
        //---------------------------------------------------------------------
        public Household(ResidentialBuilding rb, List<Resident> residents) {
            Building = rb;
            Residents = residents;
        }

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------
        public List<Resident> Residents { get; }

        private ResidentialBuilding Building { get; }

        //---------------------------------------------------------------------
        // Methods
        //---------------------------------------------------------------------
        public static Household GetRandomHousehold(ResidentialBuilding rb) {
            List<Resident> residents = new List<Resident>();
            var root = Utility.RandomGenerator.Get();
            if (root < 0.257) {
                // Pensioners
                residents.Add(new Pensioner(rb));
                if (Utility.RandomGenerator.Get() < 0.594) {
                    residents.Add(new Pensioner(rb));
                }
            } else if (root < 0.734) {
                // Families
                root = Utility.RandomGenerator.Get();
                if (root < 0.139) {
                    // Single Parent
                    if (Utility.RandomGenerator.Get() < 0.575) {
                        // Single Parent Working
                        residents.Add(new Worker(rb));
                    } else {
                        // Single Parent Unemployed
                        residents.Add(new Unemployed(rb));
                    }

                    var percentages = new List<float> {
                        0.172f, 0.472f, 0.591f, 0.771f
                    };
                    root = Utility.RandomGenerator.Get();
                    if (root < 0.691) {
                        AddChild(residents, percentages, rb);
                    } else if (root < 0.934) {
                        AddChild(residents, percentages, rb);
                        AddChild(residents, percentages, rb);
                    } else {
                        AddChild(residents, percentages, rb);
                        AddChild(residents, percentages, rb);
                        AddChild(residents, percentages, rb);
                    }
                } else {
                    // Couple Parents
                    root = Utility.RandomGenerator.Get();
                    if (root < 0.56) {
                        residents.Add(new Worker(rb));
                        residents.Add(new Worker(rb));
                    } else if (root < 0.6) {
                        residents.Add(new Unemployed(rb));
                        residents.Add(new Unemployed(rb));
                    } else {
                        residents.Add(new Worker(rb));
                        residents.Add(new Unemployed(rb));
                    }

                    root = Utility.RandomGenerator.Get();
                    if (root >= 0.456) {
                        var percentages = new List<float> { 0.301f, 0.617f, 0.72f, 0.84f };
                        if (root < 0.715) {
                            AddChild(residents, percentages, rb);
                        } else if (root < 0.929) {
                            AddChild(residents, percentages, rb);
                            AddChild(residents, percentages, rb);
                        } else {
                            AddChild(residents, percentages, rb);
                            AddChild(residents, percentages, rb);
                            AddChild(residents, percentages, rb);
                        }
                    } 
                }
            } else if (root < 0.985) {
                // Singles
                root = Utility.RandomGenerator.Get();
                if (root < 0.738) {
                    residents.Add(new Worker(rb));
                } else if (root < 0.98) {
                    residents.Add(new Unemployed(rb));
                } else {
                    residents.Add(new Student(rb));
                }
            } else {
                // Shared Flat
                residents.Add(new Student(rb));
                residents.Add(new Student(rb));
                root = Utility.RandomGenerator.Get();
                if (root > 0.5) {
                    residents.Add(new Student(rb));
                } else if (root > 0.85) {
                    residents.Add(new Student(rb));
                } else if (root > 0.95) {
                    residents.Add(new Student(rb));
                }
            }

            return new Household(rb, residents);
        }

        public override string ToString() {
            // TODO
            return "todo!";
        }

        private static void AddChild(List<Resident> residents, List<float> percentages, ResidentialBuilding rb) {
            if (percentages.Count != 4) {
                throw new ArgumentException("There need to be 4 percentages");
            }

            var percentage = Utility.RandomGenerator.Get();
            if (percentage < percentages[0]) {
                residents.Add(new Infant(rb));
            } else if (percentage < percentages[1]) {
                residents.Add(new Kid(rb));
            } else if (percentage < percentages[2]) {
                residents.Add(new Teen(rb));
            } else if (percentage < percentages[3]) {
                residents.Add(new Student(rb));
            } else {
                residents.Add(new Worker(rb));
            }
        }
    }
}
