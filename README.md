# Procedural Animation

This is a 3D Unity project made to experiment, develop and demo procedural animation using inverse kinematics. The IK (inverse kinematic) calculations only work for one joint at the moment, although with some tinkering it should be able to recursively solve more complicated IKs.

The demonstration is done using a spider-like character made through Unity, although the IKs themselves should work for any type of one-joint limbs, such as a humanoid arm or leg.

## Inverse Kinematics

### 2D

To solve an inverse kinematic of one joint, we need to find the position of the elbow. For this, we need the angle between the forward vector and the elbow joint as well as the distance to the joint. Since we know the distance to the elbow is just the length of the first bone, we only need to find the angle.

Let point $A$ be the start of the limb, point $B$ be the joint between our two bones, and point $C$ be the target our limb is trying to reach. Then we simply need to find $B$ and position each of the bones in the limb such that the first bone is going from $A$ to $B$ and the second bone is going from $B$ to $C$.

Each IK is solved using this formula derived from the law of cosines:

$$\theta_0 = { \arccos\left( { l_1^2+d^2-l_2^2\over2 l_1 d } \right) }$$

Where $l_1 =|\overrightarrow{AB}|$ is the length of the first bone, $l_2 = |\overrightarrow{BC}|$ is the length of second bone, and $d = |\overrightarrow{AC}|$ is the distance from the object to the target. This formula gives us $\theta_0$, which represents the angle from $\overrightarrow{AC}$ to $\overrightarrow{AB}$. Next we need to find the angle between the vectors $\vec{v} = d\cdot\hat{i} + 0\cdot\hat{j}$ and $\overrightarrow{AC}$. We can calculate this as 

$$\theta_1 = { \arccos\left(\overrightarrow{AC} \cdot \vec{v} \over |\overrightarrow{AC}|\left|\vec{v}\right|\right) c }$$

Where $c = {\{ A_y\le C_y : 1, A_y>C_y : -1 \}}$.

We can then calculate the angle between $\vec{v}$ and $\overrightarrow{AB}$ as $\theta = \theta_0 + \theta_1$.

This angle allows us to derive the global position of $B$ by converting from polar to cartesian coordinates and adding the new point to $A$:
$$B = A + {\left(l_1\cos(\theta),  l_1\sin(\theta)\right)}$$

Once we have calculated $B$, we can finally position our limbs in a way such that they reach the target while connected without overlapping.

[![Desmos 2D IK](images/2D-IK.png)](https://www.desmos.com/calculator/tlxbysipdl)

(click image to open in desmos)

### 3D

Moving into three dimensions may seem difficult, but it is very much the same problem. For this example, we will assume that the y-axis is in the "up" direction, as it is in Unity.

The trick to solving inverse kinematics in 3D is to rotate the limb to face the target and then once again solve the IK on a 2D plane. To do this we will need to find two angles; the angle from the y-axis to the vector $\overrightarrow{AB}$ on the xy-plane and the angle from the z-axis to the vector $\overrightarrow{AC}$ on the xz-plane. These angles will be used to find the position of $B$, as we did in 2D.

To find our first angle, $\phi$, we need to subtract the formula derived from the law of cosines from $90\degree$, or $\pi\over2$.

$$\phi = { {\pi\over2} - \arccos\left( { l_1^2+d^2-l_2^2\over2 l_1 d } \right) }$$

To confine $\overrightarrow{AC}$ to the xz-plane, we will instead use a new vector $\vec{v} = \overrightarrow{AC}.x\cdot\hat{i} + 0\cdot\hat{j} + \overrightarrow{AC}.z\cdot\hat{k}$. We can then find a new angle $\theta$ between the vectors $\vec{v}$ and $\vec{u} = 1\cdot\hat{i} + 0\cdot\hat{j} + 0\cdot\hat{k}$ using the formula

$$\theta = { \arccos\left(\vec{u} \cdot \vec{v} \over |\vec{u}|\left|\vec{v}\right|\right) c }$$

Where $c = {\{ A_z\le C_z : 1, A_z>C_z : -1 \}}$.

Once we have angles $\phi$ and $\theta$, we can then calculate the position of point $B$ relative to $A$ using a conversion from spherical to cartesian coordinates.

$$B_{relative} = \left(l_1\sin(\phi)\cos(\theta), l_1\cos(\phi), l_1\sin(\phi)\sin(\theta)\right)$$

Then we just need to add $A$ to get the position of $B$.

$$B = A + B_{relative}$$

Once we have our limbs we can then position them in such a way that they reach the target while connected and without overlapping.
