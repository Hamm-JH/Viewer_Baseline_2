using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start() :: Starting.");

        foo = "Computer Science";

        // SlowJob is as variable that contains some runction code
        // When you add the () at the end of the SlowJob variable
        // name, that's a shortcut to tell C# "execute the contents of variable SlowJob"

        myThread = new Thread( SlowJob );

        // Now run SlowJob in a new thread
        myThread.Start();

        Debug.Log("Start() :: Done.");
    }

    bool isRunnning = false;

    Thread myThread;

    string foo;

    object FrontDoor = new object();

    // Update is called once per frame
    void Update()
    {
        if (myThread.IsAlive)
            Debug.Log("SlowJob isRunning");

    }

    void PrintStudentID()
    {
        // This send "foo" to the printer
        // This should print out    "Computer Science"
        // Or maybe it'll print out "English Literature"
        // Either is legit
        // What isn't legit is if in the MIDDLE of printing, thie
        // other thread makes a change and suddenly we get a
        // student ID that says:    "Compute Literature"

        // Let's make sure no one is changing out data mid-way through
        // our printing operation.

        lock(FrontDoor)
        {
            // Print out the student ID here, safe in the
            // Knowledge that nothing is messing with our data.
        }
    }

    void SlowJob()
    {
        Debug.Log("ExampleScript::SlowJob() -- Doing 1000 things, each taking 2ms");

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        // In a Seperate thread, we decide to change the contents of Foo
        lock(FrontDoor) // If front door is already locked, the thread will PAUSE until FrontDoor is not locked
        {
            foo = "English Literature";
        }

        this.transform.position = new Vector3(5, 10, 20);

        for (int i = 0; i < 1000; i++)
        {
            Thread.Sleep(2);        // Sleep for 2ms
        }

        sw.Stop();


        // NOTE : Because of various overheads and context-switches, elapsed time
        // will be slightly more than 2 secnonds.
        Debug.Log("ExampleScript::SlowJob() -- Done! Elapsed time: " + sw.ElapsedMilliseconds / 1000f);
    }
}
