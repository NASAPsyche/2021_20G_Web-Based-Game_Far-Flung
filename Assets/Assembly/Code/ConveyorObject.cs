using System.Collections;
using UnityEngine;

public class ConveyorObject : MonoBehaviour
{
    public string ItemTypeIdentifier;
    public float LERP_Speed;
    public ConveyorSystem HostConveyor;
    public Vector3 NominalRotation; // Axis correction

    public float DisolveDurration = 1f;

    public Material DisolveMaterial;

    Vector3 Beginning;
    Vector3 Destination;

    bool AttachmentState;
    bool HeldState;
    bool finished;

    float TimeZero;
    float Distance;

    void Start() {

        Rigidbody obj = gameObject.GetComponent<Rigidbody>();
        obj.freezeRotation = false;
        obj.mass = 4;
        obj.drag = 3;
        finished = false;
        transform.rotation = Quaternion.Euler(NominalRotation);
    }

    public void InitalizeConveyorObject( Vector3 start, Vector3 end ) {

        Beginning = start;
        Destination = new Vector3(end.x, start.y, end.z);

        AttachmentState = true;
        HeldState = false;

        gameObject.SetActive(true);
        TimeZero = Time.time;
        Distance = Vector3.Distance( Beginning, Destination );
        transform.position = Beginning;
        StartCoroutine(resolveObject());
    }

    // Update is called once per frame
    void Update() {

        if ( AttachmentState ) { // if attached to the host conveyor

            float distance_covered = (Time.time - TimeZero) * LERP_Speed;
            float journey_fraction = distance_covered / Distance;
            transform.position = Vector3.Lerp(Beginning, Destination, journey_fraction); // move with respect to time

            if ( Vector3.Distance( Beginning, transform.position ) >= (Distance - 0.1) ) { // if we have reached the end of the conveyor
                StartCoroutine(removeFromConveyor());
            }
        }
    }

    private IEnumerator removeFromConveyor(){
        yield return StartCoroutine(disolveObject());//disolve the object
        gameObject.SetActive(false);
        HostConveyor.EndObjectTravel( this );
        AttachmentState = false; // no longer attached to conveyor

    }

    /*
        Disolves the object given over Disolve Durration
    */
    private IEnumerator disolveObject(){
        Material originalMat = gameObject.GetComponent<Renderer>().material;
        Material disolveMat = gameObject.GetComponent<Renderer>().material = DisolveMaterial;
        float timeStart = Time.time;
        float timeElapsed =0f;
        while(timeElapsed < DisolveDurration){
            float DisolveAmount = Mathf.Lerp(0, 1, timeElapsed/DisolveDurration); //move the disolve amount between 0 and 1 over interval
            disolveMat.SetFloat("_DisolveAmount", DisolveAmount);
            yield return null; //wait for next frame
            timeElapsed = Time.time - timeStart;
        }
        gameObject.GetComponent<Renderer>().material = originalMat;
    }

    /*
        Resolves an object (reverse disolve)
    */
    private IEnumerator resolveObject(){
        Debug.Log("Resolve Called:" + DisolveDurration + ", " + DisolveMaterial.name);
        Material originalMat = gameObject.GetComponent<Renderer>().material;
        Material disolveMat = gameObject.GetComponent<Renderer>().material = DisolveMaterial;
        float timeStart = Time.time;
        float timeElapsed =0f;
        while(timeElapsed < DisolveDurration){
            float DisolveAmount = Mathf.Lerp(1, 0, timeElapsed/DisolveDurration); //move the disolve amount between 1 and 0 over interval
            disolveMat.SetFloat("_DisolveAmount", DisolveAmount);
            yield return null; //wait for next frame
            timeElapsed = Time.time - timeStart;
        }
        gameObject.GetComponent<Renderer>().material = originalMat;
    }

    void OnCollisionEnter( Collision c ) {

        if (!HeldState && GameObject.ReferenceEquals( c.gameObject, HostConveyor.gameObject)) {

            PhysicsOff();
            InitalizeConveyorObject( transform.position, Destination );
            HostConveyor.AttachToConveyorSystem( this );
        }
    }

    public void OnPickUp() { // when the item is picked up

        HeldState = true;
        PhysicsOff();

        if ( AttachmentState ) {

            HostConveyor.DetachFromConveyorSystem( this );
            AttachmentState = false;
        }
    }

    public void OnDrop() { // when the item is released by the user

       HeldState = false;
       PhysicsOn();
    }

    void PhysicsOn() {

        Rigidbody obj = gameObject.GetComponent<Rigidbody>();
        obj.useGravity = true;
    }

    void PhysicsOff() {

        Rigidbody obj = gameObject.GetComponent<Rigidbody>();
        obj.useGravity = false;
    }

    public bool PlaceInModel() {

        bool f_state = finished;

        if (!finished) {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            finished = true;
        }
        return !f_state;
    }
}
