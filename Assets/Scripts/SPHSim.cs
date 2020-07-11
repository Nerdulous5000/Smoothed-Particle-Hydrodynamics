using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Particle
{
    public int id;
    public int cellId;
    public Vector3 position;
    // public Vector3 velocity;
    public float mass;
    public float _dens;
    public float _pres;
    public Vector3 _presForce;
    public Vector3 _viscForce;
    // public Vector3 _prevForce;
    public Vector3 _prevPosition;
}



[System.Serializable]
public class SPHSim
{
    const int numThreads = 512;
    public Vector3 MinBound;
    public Vector3 MaxBound;
    public Vector3 CellCount;
    public int NumParticles;
    public float TimeStep;

    public float H;
    public float K;
    public Vector3 Gravity;
    public float Viscosity;
    public float Mass;
    public float RestDensity;

    public Particle[] _particles;

    List<GameObject> _isnts;
    GameObject _particleInst;


    ComputeShader _shader;
    Dictionary<string, int> _shaderIds;

    ComputeBuffer _particleBuffer;
    public SPHSim(ComputeShader shader, GameObject prefab)
    {
        _shader = shader;
        _particleInst = prefab;

        _shaderIds = new Dictionary<string, int>();

        AssignShaderIDs();
    }

    ~SPHSim()
    {
        _particleBuffer.Release();

    }

    // Initial Conditions
    public void FinalizeIC()
    {
        _shader.SetInt("m_numParticles", NumParticles);
        // _shader.SetVector("m_size", Size);
        _shader.SetVector("m_minBound", MinBound);
        _shader.SetVector("m_maxBound", MaxBound);
        _shader.SetVector("m_cellCount", CellCount);
        _shader.SetFloat("m_timeStep", TimeStep);
        _shader.SetFloat("m_viscosity", Viscosity);
        _shader.SetFloat("m_h", H);
        _shader.SetVector("m_gravity", Gravity);
        _shader.SetFloat("m_restDensity", RestDensity);
        _shader.SetFloat("m_k", K);

        GenerateParticles(NumParticles);
        int particleSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Particle)); // 80
        // Debug.Log(particleSize);
        _particleBuffer = new ComputeBuffer(NumParticles, particleSize);
        Shader.SetGlobalBuffer("m_particles", _particleBuffer);
    }


    public void Simulate()
    {
        _particleBuffer.SetData(_particles);
        _shader.Dispatch(_shaderIds["SortParticles"], NumParticles / numThreads, 1, 1);
        _shader.Dispatch(_shaderIds["SetValues"], NumParticles / numThreads, 1, 1);
        _shader.Dispatch(_shaderIds["CalculateForces"], NumParticles / numThreads, 1, 1);
        _shader.Dispatch(_shaderIds["ApplyForces"], NumParticles / numThreads, 1, 1);
        _particleBuffer.GetData(_particles);
    }
    public void Refresh()
    {
        for (int n = 0; n < NumParticles; n++)
        {
            _isnts[n].transform.position = _particles[n].position;
            
        }
    }


    void AssignShaderIDs()
    {
        _shaderIds.Add("SortParticles", _shader.FindKernel("SortParticles"));
        _shaderIds.Add("SetValues", _shader.FindKernel("SetValues"));
        _shaderIds.Add("CalculateForces", _shader.FindKernel("CalculateForces"));
        _shaderIds.Add("ApplyForces", _shader.FindKernel("ApplyForces"));
    }

    void GenerateParticles(int numParticles)
    {
        // NumParticles = numParticles;
        // Particle[] Particles = new Particle[numParticles];
        // for(int n = 0; n < numParticles; n++)
        // {
        //     Particles[n] = RandomParticle(n);
        // }
        _isnts = new List<GameObject>(numParticles);
        for (int n = 0; n < NumParticles; n++)
        {
            _isnts.Add(Object.Instantiate(_particleInst));
        }

        List<Particle> particleList = new List<Particle>(NumParticles);
        for (int n = 0; n < numParticles; n++)
        {
            particleList.Add(RandomParticle(n));
        }
        _particles = particleList.ToArray();

    }


    Particle RandomParticle(int id)
    {
        Particle randParticle = new Particle();
        randParticle.id = id;
        randParticle.position = new Vector3(
                Random.Range(2.0f, MaxBound.x),
                Random.Range(2.0f, MaxBound.y),
                Random.Range(2.0f, MaxBound.z));
        randParticle._prevPosition = randParticle.position;
        // randParticle.velocity = new Vector3(0, 0, 0);
        randParticle.mass = Mass;
        return randParticle;
    }

}
