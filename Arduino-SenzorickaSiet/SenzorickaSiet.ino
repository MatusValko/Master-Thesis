#include <Firebase_Arduino_WiFiNINA.h>
#include "DHT.h"

#define FIREBASE_HOST "senzorickasiet-default-rtdb.firebaseio.com"
#define FIREBASE_AUTH "8HYBrCaWCCqZ1tdRLxenn4RDOd3mn6kZKIWbPHUM"
#define WIFI_SSID "Valko_von"
#define WIFI_PASSWORD "matusnet64"
//#define WIFI_SSID "HUAWEI-fkuf"
//#define WIFI_PASSWORD "pomarance13"

#define DHTPIN A0 //Teplota
#define MQ7pin A1 //CO
#define MQ2pin A2 //Dym
#define GL5539 A3 //Intenzita svetla
#define HW416B 2 //Pohyb

#define DHTTYPE DHT11   // DHT 11
DHT dht(DHTPIN, DHTTYPE);

FirebaseData firebaseData;

String nodeName = "Moja izba";
String path = "/Data/" + nodeName;
String jsonStr;
int counter = 0;

void setup()
{
  pinMode(HW416B, INPUT);
  Serial.begin(9600);
  Serial.print("Connecting to WiFi...");
  int status = WL_IDLE_STATUS;
  while (status != WL_CONNECTED) {
    status = WiFi.begin(WIFI_SSID, WIFI_PASSWORD);
    Serial.print(".");
    delay(300);
  }
  Serial.print(" IP: ");
  Serial.println(WiFi.localIP());
  Serial.println();

  Firebase.begin(FIREBASE_HOST, FIREBASE_AUTH, WIFI_SSID, WIFI_PASSWORD);
  Firebase.reconnectWiFi(true);

  SetUpDB(path);
  dht.begin();
  delay(200);
}

void loop()
{
  SendData(path,nodeName,counter);

  if(counter >= 240){
    counter = 0;
  }
  counter++;
  delay(15000);
}

void SetUpDB(String path) {
  //Teplomer
  if (Firebase.setString(firebaseData, path + "/Teplomer/Jednotka", "°C")) {
      Serial.println(firebaseData.dataPath() + " = " + "°C");
    }
  if (Firebase.setString(firebaseData, path + "/Teplomer/Velicina", "Teplota")) {
      Serial.println(firebaseData.dataPath() + " = " + "Teplota");
    }
  //Vlhkomer
  if (Firebase.setString(firebaseData, path + "/Vlhkomer/Jednotka", "%")) {
      Serial.println(firebaseData.dataPath() + " = " + "%");
    }
  if (Firebase.setString(firebaseData, path + "/Vlhkomer/Velicina", "Vlhkosť")) {
      Serial.println(firebaseData.dataPath() + " = " + "Vlhkosť");
    }
  //Svetlomer
  if (Firebase.setString(firebaseData, path + "/Svetlomer/Jednotka", "Lx")) {
      Serial.println(firebaseData.dataPath() + " = " + "Lx");
    }
  if (Firebase.setString(firebaseData, path + "/Svetlomer/Velicina", "Intenzita osvetlenia")) {
      Serial.println(firebaseData.dataPath() + " = " + "Osvetlenie");
    }
  //Senzor dymu
  if (Firebase.setString(firebaseData, path + "/Senzor dymu/Jednotka", "Ppm")) {
      Serial.println(firebaseData.dataPath() + " = " + "Ppm");
    }
  if (Firebase.setString(firebaseData, path + "/Senzor dymu/Velicina", "Dym")) {
      Serial.println(firebaseData.dataPath() + " = " + "Dym");
    }
  //Oxid uhoľnatý
  if (Firebase.setString(firebaseData, path + "/Detektor Co/Jednotka", "Ppm")) {
      Serial.println(firebaseData.dataPath() + " = " + "Ppm");
    }
  if (Firebase.setString(firebaseData, path + "/Detektor Co/Velicina", "Oxid uhoľnatý")) {
      Serial.println(firebaseData.dataPath() + " = " + "Oxid uhoľnatý");
    }

  //Hlukomer
  if (Firebase.setString(firebaseData, path + "/Hlukomer/Jednotka", "Db")) {
        Serial.println(firebaseData.dataPath() + " = " + "Db");
    }
  if (Firebase.setString(firebaseData, path + "/Hlukomer/Velicina", "Intenzita zvuku")) {
        Serial.println(firebaseData.dataPath() + " = " + "Intenzita zvuku");
     }
  //Detektor Co2
  if (Firebase.setString(firebaseData, path + "/Detektor Co2/Jednotka", "Ppm")) {
        Serial.println(firebaseData.dataPath() + " = " + "Ppm");
    }
  if (Firebase.setString(firebaseData, path + "/Detektor Co2/Velicina", "Oxid uhličitý")) {
        Serial.println(firebaseData.dataPath() + " = " + "Oxid uhličitý");
     }
  //Detektor pohybu   
  if (Firebase.setString(firebaseData, path + "/Detektor pohybu/Jednotka", "Pohyb")) {
        Serial.println(firebaseData.dataPath() + " = " + "Pohyb");
    }
  if (Firebase.setString(firebaseData, path + "/Detektor pohybu/Velicina", "Pohyb")) {
        Serial.println(firebaseData.dataPath() + " = " + "Pohyb");
     }

}

void SendData(String path,String nodeName,int counter){
  Firebase.setTimestamp(firebaseData, path +"/Timestamp") ? "ok" : firebaseData.errorReason().c_str();
  String timestamp = firebaseData.payload();
  Serial.println("Timestamp: " + timestamp);
  

  //TEPLOTA
  int t = dht.readTemperature();
  if (Firebase.setFloat(firebaseData, path + "/Teplomer/Hodnota", t)) {
      Serial.println(firebaseData.dataPath() + " = " + t);
    }
  //VLHKOSŤ
  int h = dht.readHumidity();
  if (Firebase.setFloat(firebaseData, path + "/Vlhkomer/Hodnota", h)) {
      Serial.println(firebaseData.dataPath() + " = " + h);
    }
  //SVETLOMER
  int value = analogRead(GL5539);
  if (Firebase.setFloat(firebaseData, path + "/Svetlomer/Hodnota",value)) {
        Serial.println(firebaseData.dataPath() + " = " + value);
    }
  //SENZOR DYMU, Treshold = 400
  int sensorValue2 = analogRead(MQ2pin);
  if (Firebase.setFloat(firebaseData, path + "/Senzor dymu/Hodnota", sensorValue2)) {
        Serial.println(firebaseData.dataPath() + " = " + sensorValue2);
    }
  //HLUKOMER
  int randomNumber3 = random(100);
  if (Firebase.setFloat(firebaseData, path + "/Hlukomer/Hodnota", randomNumber3)) {
        Serial.println(firebaseData.dataPath() + " = " + randomNumber3);
     }
  //DETEKTOR Co
  int sensorValue = analogRead(MQ7pin);
  if (Firebase.setFloat(firebaseData, path + "/Detektor Co/Hodnota", sensorValue)) {
        Serial.println(firebaseData.dataPath() + " = " + sensorValue);
    }
  //DETEKTOR Co2
  int co2 = sensorValue /2;
  if (Firebase.setFloat(firebaseData, path + "/Detektor Co2/Hodnota", co2)) {
        Serial.println(firebaseData.dataPath() + " = " + co2);
     }  
  //POHYB
  int val = digitalRead(HW416B);
  int invert = 0;
  if (val == 0){
    //Serial.println("Motion detected!"); 
    invert = 1;
  }
  else {
    //Serial.println("Motion stopped!"); 
    invert = 0;
  }
  
  if(Firebase.setFloat(firebaseData, path + "/Detektor pohybu/Hodnota", invert)) {
        Serial.println(firebaseData.dataPath() + " = " + invert);
    }

//POSLI ZAZNAM DO LOGU
  if(counter >= 240){
    jsonStr = "{\"Teplomer\":{\"Hodnota\":" + String(t) + ", \"Jednotka\": \"°C\", \"Velicina\": \"Teplota\"}"+
              ",\"Vlhkomer\":{\"Hodnota\":" + String(h) + ", \"Jednotka\": \"%\", \"Velicina\": \"Vlhkosť\"}"+
              ",\"Svetlomer\":{\"Hodnota\":" + String(value) + ", \"Jednotka\": \"Lux\", \"Velicina\": \"Intenzita osvetlenia\"}"+
              ",\"Senzor dymu\":{\"Hodnota\":" + String(sensorValue2) + ", \"Jednotka\": \"Ppm\", \"Velicina\": \"Dym\"}"+
              ",\"Hlukomer\":{\"Hodnota\":" + String(randomNumber3) + ", \"Jednotka\": \"Db\", \"Velicina\": \"Intenzita zvuku\"}"+
              ",\"Detektor Co\":{\"Hodnota\":" + String(sensorValue) + ", \"Jednotka\": \"Ppm\", \"Velicina\": \"Oxid uhoľnatý\"}"+
              ",\"Detektor Co2\":{\"Hodnota\":" + String(co2) + ", \"Jednotka\": \"Ppm\", \"Velicina\": \"Oxid uhličitý\"}"+
              ",\"Detektor pohybu\":{\"Hodnota\":" + String(invert) + ", \"Jednotka\": \"Pohyb\", \"Velicina\": \"Pohyb\"}"+

              ",\"Timestamp\":" + timestamp +"}";

    Serial.println(jsonStr);

    if (Firebase.pushJSON(firebaseData, "/Logs/" + nodeName, jsonStr)) {
      Serial.println(firebaseData.dataPath() + " = " + firebaseData.pushName());
    }
    else {
      Serial.println("Error: " + firebaseData.errorReason());
    }
  }
}