#version 330

attribute vec4 aVert;

varying vec4 vColor;

uniform mat4 uPVMMatrix;
uniform int uColorAxes;

void main(void)
{
	gl_Position = uPVMMatrix * vec4(aVert.x, aVert.y, -aVert.z, 1);

	vColor = vec4(0, 0, 0, 1);
	if(uColorAxes != 0)
	{
		switch(int(aVert.w))
		{
			default:
				break;
			case 1:
				vColor = vec4(1, 0, 0, 1);
				break;
			case 2:
				vColor = vec4(0, 1, 0, 1);
				break;
			case 3:
				vColor = vec4(0, 0, 1, 1);
				break;
		}
	}
}