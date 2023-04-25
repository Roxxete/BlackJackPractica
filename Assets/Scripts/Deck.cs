using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;
    public Text PointsText;
    public Text DealerPointsText;
    public float totalcartas;
    public float mayor21;
    public float entre17y21;
    public float dealermayor;

    public int[] values = new int[52];
    int cardIndex = 0;
   
    

    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();
    }
    
    private void InitCardValues()
    {
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */
        for(int i =0; i < faces.Length; i++)
        {
            int InitCardValues = (i % 13) + 1;
            if (InitCardValues > 10)
            {
                InitCardValues = 10;
            }
            if(InitCardValues == 1)
            {
                InitCardValues = 11;
            }
            values[i] = InitCardValues;
        }

    }


    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */
        int n = faces.Length;
        for (int i = 0; i < n - 1; i++)
        {
            int r = Random.Range(i, n);
            Sprite tempFace = faces[i];
            int tempValue = values[i];
            faces[i] = faces[r];
            values[i] = values[r];
            faces[r] = tempFace;
            values[r] = tempValue;
        }
    }

    void StartGame()
    {
        
        DealerPointsText.text = "";
        /*TODO:
         * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
         */

        for (int i = 0; i < 2; i++)
            {
                PushPlayer();
                PushDealer();

                // Verificar si alguno de los dos tiene Blackjack
               
                    if (player.GetComponent<CardHand>().points == 21 && dealer.GetComponent<CardHand>().points == 21)
                    {
                        hitButton.interactable = false;
                        stickButton.interactable = false;
                        playAgainButton.interactable = true;
                        finalMessage.text = "Ambos tienen Blackjack. Empate.";
                dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
                DealerPointsText.text = "Puntos: " + dealer.GetComponent<CardHand>().points;
                return;
                    }
                    else if (player.GetComponent<CardHand>().points == 21)
                    {
                        hitButton.interactable = false;
                        stickButton.interactable = false;
                        playAgainButton.interactable = true;
                        finalMessage.text = "Has ganado.";
                dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
                DealerPointsText.text = "Puntos: " + dealer.GetComponent<CardHand>().points;
                return;
                    }
            else if (dealer.GetComponent<CardHand>().points == 21)
            {
                        hitButton.interactable = false;
                        stickButton.interactable = false;
                        playAgainButton.interactable = true;
                        finalMessage.text = "Has perdido";
                dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
                DealerPointsText.text = "Puntos: " + dealer.GetComponent<CardHand>().points;
                return;
                    }
                
            }

        PointsText.text = "Puntos: " + player.GetComponent<CardHand>().points;
        }


    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
        // Calcular la probabilidad de que el crupier tenga más puntuación que el jugador con la carta oculta
        

            totalcartas = 0;
            float puntoplayer = player.GetComponent<CardHand>().points;
            mayor21 = 0;
            entre17y21 = 0;
            dealermayor = 0;

            for (int i = cardIndex + 1; i < faces.Length; i++)
            {
                totalcartas++;

                if (values[3] + values[i] > puntoplayer)
                {
                    dealermayor++;
                }


                if (values[i] + puntoplayer >= 17 && values[i] + puntoplayer <= 21)
                {
                    entre17y21++;
                }


                if (values[i] + puntoplayer > 21)
                {
                    mayor21++;
                }
            }

            probMessage.text =
            "Dealer > Player: " + dealermayor / totalcartas + "\n" +
            "17 <= X <= 21: " + entre17y21 / totalcartas + "\n" +
            "21 > X: " + mayor21 / totalcartas;


    }

    void PushDealer()
    {
        Debug.Log("Dealer");
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]);
        cardIndex++;

    }

    void PushPlayer()
    {
        Debug.Log("player");
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]);
        cardIndex++;
        CalculateProbabilities();
        PointsText.text = "Puntos: " + player.GetComponent<CardHand>().points;
    }       

    public void Hit()
    {
        
        
        PushPlayer();
        //GANAS
        if (player.GetComponent<CardHand>().points == 21)
        {
            finalMessage.text = "Blackjack!!";
            hitButton.interactable = false;
            stickButton.interactable = false;
            playAgainButton.interactable = true;
            DealerPointsText.text = "Puntos: " + dealer.GetComponent<CardHand>().points;
            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
        }
        //PIERDES
        if(player.GetComponent<CardHand>().points > 21)
        {
            finalMessage.text = "Has perdido";
            hitButton.interactable = false;
            stickButton.interactable = false;
            playAgainButton.interactable = true;
            DealerPointsText.text = "Puntos: " + dealer.GetComponent<CardHand>().points;
            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
        }
        
    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */
        dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
        hitButton.interactable = false;
        stickButton.interactable = false;
        while(dealer.GetComponent<CardHand>().points < 17)
        {
            PushDealer();
        }

       
        if (dealer.GetComponent<CardHand>().points == 21)
        {
            finalMessage.text = "Has perdido";
            hitButton.interactable = false;
            stickButton.interactable = false;
            playAgainButton.interactable = true;
            DealerPointsText.text = "Puntos: " + dealer.GetComponent<CardHand>().points;

        }
        if (dealer.GetComponent<CardHand>().points > player.GetComponent<CardHand>().points)
        {
            finalMessage.text = "Has perdido";
            hitButton.interactable = false;
            stickButton.interactable = false;
            playAgainButton.interactable = true;
            DealerPointsText.text = "Puntos: " + dealer.GetComponent<CardHand>().points;
        }
        if (dealer.GetComponent<CardHand>().points < player.GetComponent<CardHand>().points)
        {
            finalMessage.text = "Has ganado";
            hitButton.interactable = false;
            stickButton.interactable = false;
            playAgainButton.interactable = true;
            DealerPointsText.text = "Puntos: " + dealer.GetComponent<CardHand>().points;
        }
        if (dealer.GetComponent<CardHand>().points == player.GetComponent<CardHand>().points)
        {
            finalMessage.text = "Empate";
            hitButton.interactable = false;
            stickButton.interactable = false;
            playAgainButton.interactable = true;
            DealerPointsText.text = "Puntos: " + dealer.GetComponent<CardHand>().points;
        }
        if(dealer.GetComponent<CardHand>().points > 21)
        {
            finalMessage.text = "Has ganado";
            hitButton.interactable = false;
            stickButton.interactable = false;
            playAgainButton.interactable = true;
            DealerPointsText.text = "Puntos: " + dealer.GetComponent<CardHand>().points;
        }
    }

        void bet()
        {

        }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }

}