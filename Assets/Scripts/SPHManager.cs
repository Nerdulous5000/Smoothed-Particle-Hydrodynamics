using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SPHManager : MonoBehaviour
{
    // Start is called before the first frame update

    public ComputeShader shader;
    public GameObject inst;
    public SPHSim sim;
    void Start()
    {
        sim = new SPHSim(shader, inst);
        sim.MinBound = new Vector3(1, 1, 1);
        sim.MaxBound = new Vector3(4, 4, 4);
        sim.CellCount = new Vector3(1, 1, 1);
        sim.TimeStep = .05f;

        sim.Mass = 5.0f;
        sim.NumParticles = 1024 * 2;

        sim.Viscosity = 1f;
        sim.RestDensity = 82.0f;
        sim.Gravity = Physics.gravity;
        sim.H = .7f;
        sim.K = 50.0f;

        sim.FinalizeIC();


    }

    void Update()
    {
        sim.Simulate();
        sim.Refresh();
    }


}
