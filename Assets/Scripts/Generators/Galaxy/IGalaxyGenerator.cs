using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGalaxyGenerator
{
    Galaxy Generate(int seed);
}
