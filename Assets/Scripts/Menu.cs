using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject _hero;
    [SerializeField]
    private GameObject _tanker;
    [SerializeField]
    private GameObject _ranger;
    [SerializeField]
    private GameObject _soldier;



    private Animator _tankerAnim;
    private Animator _rangerAnim;
    private Animator _soldierAnim;
    private Animator _heroAnim;

    private void Start()
    {
        _heroAnim = _hero.GetComponent<Animator>();
        _tankerAnim = _tanker.GetComponent<Animator>();
        _rangerAnim = _ranger.GetComponent<Animator>();
        _soldierAnim = _soldier.GetComponent<Animator>();

        MenuAnimations();
    }

    private void MenuAnimations()
    {
        _heroAnim.Play("MenuSpinAttack");
        _tankerAnim.Play("MenuAttackTanker");
        _rangerAnim.Play("MenuAttackTanker");
        _soldierAnim.Play("MenuAttackTanker");
    }

    public void BattleButton()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
