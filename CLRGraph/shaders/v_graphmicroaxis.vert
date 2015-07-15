#version 330

attribute vec4 aVert;

varying vec4 vColor;

uniform mat4 uPVMMatrix;

void main(void)
{
	vec4 vPos = uPVMMatrix * vec4(aVert.x, aVert.y, -aVert.z, 1);
	vPos.xy -= 0.9;
	gl_Position = vPos;

	switch(int(aVert.w))
	{
		default:
		case 0:
			vColor = vec4(0, 0, 0, 1);
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