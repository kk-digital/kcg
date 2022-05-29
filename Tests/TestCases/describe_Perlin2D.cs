using PerlinNoise;
using NSpec;

public class describe_Perlin2D: nspec 
{

    void when_executing()
    {
        it["grad has more than 1 array item"] = () =>
        {
            // given
            var perlinField2D = new PerlinField2D();
            perlinField2D.init(1, 5);
            // when
            perlinField2D.generate_gradient_array();
            // then
            perlinField2D.grad.Length.should_be_greater_or_equal_to(1);

        };
    }

}