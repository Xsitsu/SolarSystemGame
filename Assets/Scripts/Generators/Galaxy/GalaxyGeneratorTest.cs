using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyGeneratorTest : IGalaxyGenerator
{
    Star GenerateSystem(ISystemGenerator generator, string name, double orbitRadius_LY)
    {
        Star sol = generator.Generate();
        sol.name = name;
        sol.semiMajorAxis_m = orbitRadius_LY * Numbers.LightYearToKM * Numbers.KMToM;
        return sol;
    }
    public Galaxy Generate(int seed)
    {
        ISystemGenerator systemGenerator = new SystemGeneratorSol();

        Galaxy galaxy = new Galaxy()
        {
            name = "Milky Way",
            mass_kg = 4.1 * 1000000 * Numbers.SolarMassToKG,
        };

        galaxy.AddChild(GenerateSystem(systemGenerator, "Sol 1", 100));
        galaxy.AddChild(GenerateSystem(systemGenerator, "Sol 2", 96));
        galaxy.AddChild(GenerateSystem(systemGenerator, "Sol 3", 85));
        galaxy.AddChild(GenerateSystem(systemGenerator, "Sol 4", 128));
        galaxy.AddChild(GenerateSystem(systemGenerator, "Sol 5", 91));
        galaxy.AddChild(GenerateSystem(systemGenerator, "Sol 6", 112));

        return galaxy;
    }
}
