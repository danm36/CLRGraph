#version 330

attribute vec3 aVertex;
attribute vec3 aNormal;

varying vec4 vPosition;
varying vec3 vNormal;

uniform mat4 uPVMMatrix;
uniform mat4 uVertexOffset;

void main(void)
{
	gl_Position = uPVMMatrix * (uVertexOffset * vec4(aVertex.x, aVertex.y, -aVertex.z, 1));
	vPosition = vec4(aVertex, 1);
	vNormal = aNormal;
}