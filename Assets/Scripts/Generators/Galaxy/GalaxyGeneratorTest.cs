using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyGeneratorTest : IGalaxyGenerator
{
    Star GenerateSystem(ISystemGenerator generator, string name, double orbitRadiusLY, double orbitPercentOffset)
    {
        Star sol = generator.Generate();
        sol.name = name;
        sol.orbitRadius = orbitRadiusLY * Numbers.LightYearToKM * 1000;
        sol.orbitPercentOffset = orbitPercentOffset;
        return sol;
    }
    public Galaxy Generate(int seed)
    {
        ISystemGenerator systemGenerator = new SystemGeneratorSol();

        Galaxy galaxy = new Galaxy();
        galaxy.name = "Milky Way";
        galaxy.mass = 4.1 * 1000000 * Numbers.SolarMassToKG;

        galaxy.AddSatellite(GenerateSystem(systemGenerator, "Sol 1", 100, 0.47));
        galaxy.AddSatellite(GenerateSystem(systemGenerator, "Sol 2", 96, 0.47));
        galaxy.AddSatellite(GenerateSystem(systemGenerator, "Sol 3", 85, 0));
        galaxy.AddSatellite(GenerateSystem(systemGenerator, "Sol 4", 128, 0.32));
        galaxy.AddSatellite(GenerateSystem(systemGenerator, "Sol 5", 91, 0.83));
        galaxy.AddSatellite(GenerateSystem(systemGenerator, "Sol 6", 112, 0.67));

        return galaxy;
    }
}
