public class Marker {

        private int ID;
        private float posX;
        private float posY;
        private float angle;

        public Marker(int ID, float posX, float posY, float angle){
            this.ID = ID;
            this.posX = posX;
            this.posY = posY;
            this.angle = angle;
        }

        public int getID(){
            return this.ID;
        }

        public float getPosX(){
            return this.posX;
        }

        public float getPosY(){
            return this.posY;
        }

        public float getAngle(){
            return this.angle;
        }

        public void setID(int ID){
            this.ID = ID;
        }

        public void setPosX(float posX){
            this.posX = posX;
        }

        public void setPosY(float posY){
            this.posY = posY;
        }

        public void setAngle(float angle){
            this.angle = angle;
        }

    public string toStr()
        {
            return "Marker " + this.ID + " data:\n" +
                "\tPosition: (" + this.posX + "/" + this.posY + ")\n" +
                "\tAngle: " + this.angle;
        }

    void Start () {}

    void Update () {}
}
