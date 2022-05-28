using NSpec;
class describe_tests: nspec {
    void when_testing() {
        it["fails"] = () => {
            false.should_be_true();
        };

        it["works"] = () => {
            true.should_be_true();
        };
    }
}