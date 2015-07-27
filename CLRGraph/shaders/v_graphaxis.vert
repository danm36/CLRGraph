#version 330

attribute vec4 aVertex;

varying vec4 vColor;

uniform mat4 uPVMMatrix;
uniform int uColorAxes;

void main(void)
{
	gl_Position = uPVMMatrix * vec4(aVertex.x, aVertex.y, -aVertex.z, 1);

	vColor = vec4(0, 0, 0, 1);
	if(uColorAxes != 0)
	{
		switch(int(aVertex.w))
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