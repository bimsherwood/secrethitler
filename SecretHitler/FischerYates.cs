namespace SecretHitler;

public static class FischerYates {

    public static void Shuffle<T>(Random rand, IList<T> list){
        for (int i = list.Count - 1; i > 0; i--) {

            // Pick a random index from 0 to i
            int j = rand.Next(0, i+1);
    
            // Swap arr[i] with a random element not further along the array
            var a = list[i];
            var b = list[j];
            list[i] = b;
            list[j] = a;

        }
    }

}
