shader_type spatial;

uniform float t = 0.0;

void fragment() {
	vec2 uv = 2. * UV - 1.;
	vec2 uv2 = uv * (1. - 0.3 * pow(sin(3. * (t - 0.25)), 16.));
	float center = smoothstep(0.52, 0.5, length(1.06 * uv2));
	uv *= 0.52 + 0.72 * t;
	ALBEDO = vec3(1.);
	ALPHA = smoothstep(0., 0.15, t) * smoothstep(1., 0.85, t) * max(center, smoothstep(0.52, 0.5, 1.-length(uv)) * smoothstep(0.48, 0.5, 1.-length(uv)));
}
