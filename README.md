# Smoothed-Particle-Hydrodynamics

**Credit:**  
Staubach, D. (Dec 22, 2010). Smoothed Particle Hydrodynamics Real-Time Fluid Simulation Approach (Bachelor Thesis, University of Erlangen-Nuremberg, Erlangen, Germany). Retrieved from https://www10.cs.fau.de/publications/theses/2010/Staubach_BT_2010.pdf  
  
## Summary
A demonstration of SPH(Smoothed-Particle-Hydrodynamics) using Unity 2018.4.24f1. SPH simulates a series of internal and external forces on a particle, which influences similar forces on adjacent particles.

### Implemented Forces: ###
#### Internal- ####
* Pressure  
* Viscosity  
#### External- ####
* Gravity  

## Gallery ##
![alt text](https://cdn.discordapp.com/attachments/688514381650198528/731294308329586728/unknown.png)
  
## Initial Conditions ##
To replicate similar results, the following initial conditions were used. (These numbers were not chosen to be accurate, but were found to provide an observable simulation)
| Symbol  | Description | Value |
| ------- | ------------- | ------------- |
| n              | Particle Count       |  2048                                 |
| Δt             | Time Step            |  0.05s                                |
| m              | Mass                 |  5.0g                                 |
| g              | Gravity              | -9.81m·s<sup>-2</sup>                |
| μ              | Viscosity            |  1.0 kg·m<sup>-1</sup>·s<sup>-1</sup> |
| ρ<sub>0</sub>  | Resting Density      |  82.0 kg·m<sup>-3</sup>               |
| k              | Ideal Gas Stiffness  |  50.0                                 |
| h              | Weighting Radius     |  0.7                                  |

## TODO ##  
The following features are planned additions
* Implement grid partitioning to lower calculations
* Integrate with Unity's Rigidbody system  
