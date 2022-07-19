using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Platform : MonoBehaviour
{
    Animator anim;

    [Header("Rotation speed")]
    public float speed_rot;

    [Header("Movement speed during jump")]
    public float speed_move;

    [Header("Time available for combo")]
    public int term;

    public bool isJump;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Rotate();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        if (!isJump)
        {            
            Attack();
            
            Dodge();
            
            Jump();

            Block();
            
            Skill1();
            
            Skill2();
            
            Skill3();
            
            Skill4();
            
            Skill5();
            
            Skill6();
            
            Skill7();
            
            Skill8();
        }
    }

    Quaternion rot;
    bool isRun;

    
    void Rotate()
    {
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
        {            
            Move();            
            rot = Quaternion.LookRotation(Vector3.right);
        }

        
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S))
        {            
            Move();
            rot = Quaternion.LookRotation(Vector3.left);
        }

        else
        {            
            anim.SetBool("Run", false);
        }
        
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, speed_rot * Time.deltaTime);

    }

    
    void Move()
    {
        
        if (isJump)
        {            
            transform.position += transform.forward * speed_move * Time.deltaTime;            
            anim.SetBool("Run", false);
        }
        else
        {            
            anim.SetBool("Run", true);
        }
    }

    int clickCount;
    float timer;
    bool isTimer;

    
    void Attack()
    {
        
        if (isTimer)
        {
            timer += Time.deltaTime;
        }

        
        if (Input.GetMouseButtonDown(0))
        {
            switch (clickCount)
            {
                
                case 0:
                    
                    anim.SetTrigger("Attack1");
                    
                    isTimer = true;
                    
                    clickCount++;
                    break;

                
                case 1:
                    
                    if (timer <= term)
                    {                        
                        anim.SetTrigger("Attack2");
                        
                        clickCount++;
                    }

                    
                    else
                    {                        
                        anim.SetTrigger("Attack1");
                        
                        clickCount = 1;
                    }

                    
                    timer = 0;
                    break;

                
                case 2:
                    
                    if (timer <= term)
                    {                        
                        anim.SetTrigger("Attack3");
                        
                        clickCount = 0;
                        
                        isTimer = false;
                    }

                    
                    else
                    {                        
                        anim.SetTrigger("Attack1");
                        
                        clickCount = 1;
                    }
                
                    timer = 0;
                    break;
            }
        }
    }

    
    void Dodge()
    {
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {            
            anim.SetTrigger("Dodge");
        }
    }

    void Block()
    {

        if (Input.GetMouseButton(1))
        {
            anim.SetBool("Block", true);
        }
        else
        {
            anim.SetBool("Block", false);
        }
    }


    void Jump()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {            
            anim.SetTrigger("Jump");

            isJump = true;
        }
    }

    
    void JumpEnd()
    {
        isJump = false;
    }

    // Skill1
    void Skill1()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Play Skill1 animation
            anim.SetTrigger("Skill1");
        }
    }
    // Skill2
    void Skill2()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Play Skill2 animation
            anim.SetTrigger("Skill2");
        }
    }
    // Skill3
    void Skill3()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Play Skill3 animation
            anim.SetTrigger("Skill3");
        }
    }
    // Skill4
    void Skill4()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Play Skill4 animation
            anim.SetTrigger("Skill4");
        }
    }
    // Skill5
    void Skill5()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // Play Skill5 animation
            anim.SetTrigger("Skill5");
        }
    }
    // Skill6
    void Skill6()
    {
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // Play Skill6 animation
            anim.SetTrigger("Skill6");
        }
    }
    // Skill7
    void Skill7()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            // Play Skill7 animation
            anim.SetTrigger("Skill7");
        }
    }
    // Skill8
    void Skill8()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            // Play Skill8 animation
            anim.SetTrigger("Skill8");
        }
    }
}
