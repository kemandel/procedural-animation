# Procedural Animation

This is a 3D Unity project made to experiment, develop and demo procedural animation using inverse kinematics. The IK (inverse kinematic) calculations only work for one joint at the moment, although with some tinkering it should be able to recursively solve more complicated IKs.

The demonstration is done using a spider-like character made through Unity, although the IKs themselves should work for any type of one-joint limbs, such as a humanoid arm or leg.

## Inverse Kinematics

### 2D

Let point $A$ be the `body` of the limb, point $B$ be the joint between our two limbs, point $C$ be the target our limb is trying to reach. Then we simply need to find $B$ and position each of 
Each IK is solved using this formula derived from the law of cosines:
<!-- $$
\theta_0 = { \arccos\left( { l_1^2+d^2-l_2^2\over2 l_1 d } \right) }
$$ --> 

<div align="center"><img style="background: white;" src="svg\zlWwaB4i6u.svg"></div>

Where <!-- $l_1$ --> <img style="transform: translateY(0.1em); background: white;" src="svg\LThvZL0SNl.svg"> is the length of the first `bone`, <!-- $l_2$ --> <img style="transform: translateY(0.1em); background: white;" src="svg\okrg28Ou1q.svg"> is the length of second `bone`, and <!-- $d$ --> <img style="transform: translateY(0.1em); background: white;" src="svg\zNbhCWMPAY.svg"> is the distance from the object to the target. This formula gives us <!-- $\theta_0$ --> <img style="transform: translateY(0.1em); background: white;" src="svg\6YKqVBVbmx.svg">, which represents the angle from the x-axis <!-- $\left<d,0\right>$ --> <img style="transform: translateY(0.1em); background: white;" src="svg\UBt1XT9pQD.svg"> and <!-- $\overrightarrow{AB}$ --> <img style="transform: translateY(0.1em); background: white;" src="svg\cbxgC7mzU3.svg">\. Next we need to find the angle between the vectors <!-- $\overrightarrow{AC}$ --> <img style="transform: translateY(0.1em); background: white;" src="svg\325INYv7sL.svg"> and the x-axis <!-- $\left<d,0\right>$ --> <img style="transform: translateY(0.1em); background: white;" src="svg\JaE7H8yFHX.svg">. We can calculate this as 
<!-- $$
\theta_1 = { \arccos\left(\overrightarrow{AC} \cdot \left<d,0\right> \over |\overrightarrow{AC}|\left|\left<d,0\right>\right|\right) c }
$$ --> 

<div align="center"><img style="background: white;" src="svg\E2gswxJD6m.svg"></div>

Where <!-- $c = {\{ A_y < C_y : 1, A_y > C_y : -1 \}}$ --> <img style="transform: translateY(0.1em); background: white;" src="svg\jy0x4HzbBF.svg"> .

We can then calculate the angle between <!-- $\overrightarrow{AC}$ --> <img style="transform: translateY(0.1em); background: white;" src="svg\ieGNcDC98n.svg"> and <!-- $\overrightarrow{AB}$ --> <img style="transform: translateY(0.1em); background: white;" src="svg\bZOklRSeXt.svg"> as <!-- $\theta = \theta_0 + \theta_1$ --> <img style="transform: translateY(0.1em); background: white;" src="svg\mYwCcF00qN.svg">

This angle allows us to derive the global position of <!-- $B$ --> <img style="transform: translateY(0.1em); background: white;" src="svg\ifumTSKfgH.svg"> by converting from polar to cartesian coordinates and adding <!-- $A$ --> <img style="transform: translateY(0.1em); background: white;" src="svg\JqlCG8than.svg"> in the equation
<!-- $$ 
B = A + {\left(cos(\theta) * l_1, sin(\theta) * l_1\right)} 
$$ --> 

<div align="center"><img style="background: white;" src="svg\7ptjwm1F99.svg"></div>

Once we have calculated <!-- $B$ --> <img style="transform: translateY(0.1em); background: white;" src="svg\OEi4kukbVI.svg"> , we can finally position our limbs in a way such that they reach the target without overlapping.

![image info](images/2D-IK.png)
