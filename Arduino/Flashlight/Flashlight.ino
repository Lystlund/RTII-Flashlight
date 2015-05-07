// I2C device class (I2Cdev) demonstration Arduino sketch for MPU9150
// 1/4/2013 original by Jeff Rowberg <jeff@rowberg.net> at https://github.com/jrowberg/i2cdevlib
//          modified by Aaron Weiss <aaron@sparkfun.com>
//
// Changelog:
//     2011-10-07 - initial release
//     2013-1-4 - added raw magnetometer output

/* ============================================
I2Cdev device library code is placed under the MIT license

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
===============================================
*/

// Arduino Wire library is required if I2Cdev I2CDEV_ARDUINO_WIRE implementation
// is used in I2Cdev.h
#include "Wire.h"

// I2Cdev and MPU6050 must be installed as libraries, or else the .cpp/.h files
// for both classes must be in the include path of your project
#include "I2Cdev.h"
#include "MPU6050.h"

// class default I2C address is 0x68
// specific I2C addresses may be passed as a parameter here
// AD0 low = 0x68 (default for InvenSense evaluation board)
// AD0 high = 0x69
MPU6050 accelgyro;

int16_t ax, ay, az;
int16_t gx, gy, gz;
int16_t mx, my, mz;

int Cax, Cay, Caz, Cgx, Cgy, Cgz, Cmx, Cmy, Cmz;

#define LED_PIN 13
bool blinkState = false;
const int buttonPin01 = 2;

void setup() {
    // join I2C bus (I2Cdev library doesn't do this automatically)
    Wire.begin();

    // initialize serial communication
    // (38400 chosen because it works as well at 8MHz as it does at 16MHz, but
    // it's really up to you depending on your project)
    Serial.begin(38400);

    pinMode(buttonPin01, INPUT);
    digitalWrite(buttonPin01, LOW);

    // initialize device
    //Serial.println("Initializing I2C devices...");
    accelgyro.initialize();

    // verify connection
   // Serial.println("Testing device connections...");
    //Serial.println(accelgyro.testConnection() ? "MPU6050 connection successful" : "MPU6050 connection failed");

    // configure Arduino LED for
    pinMode(LED_PIN, OUTPUT);
}

void loop() {
  
    // read raw accel/gyro measurements from device
    accelgyro.getMotion9(&ax, &ay, &az, &gx, &gy, &gz, &mx, &my, &mz);
   
    // these methods (and a few others) are also available
    //accelgyro.getAcceleration(&ax, &ay, &az);
    //accelgyro.getRotation(&gx, &gy, &gz);
    float potentiometer = analogRead(A1);


    // display tab-separated accel/gyro x/y/z values
    //Serial.print("a/g/m:\t");
    
    //Cax = runningAverage(ax);
    
    //ax = ax - Cax;
        
    Serial.print(map(ax, 0, 65535, 0, 359)-1); Serial.print(", ");
    Serial.print(map(ay, 0, 65535, 0, 359)+1); Serial.print(", ");
    Serial.print(map(az, 0, 65535, 0, 359)-85); Serial.print(", ");
    Serial.print(map(gx, 0, 65535, 0, 359)); Serial.print(", ");
    Serial.print(map(gy, 0, 65535, 0, 359)+1); Serial.print(", ");
    Serial.print(map(gz, 0, 65535, 0, 359)); Serial.print(", ");
    Serial.print(map(mx, 0, 4095, 0, 359)+9); Serial.print(", ");
    Serial.print(map(my, 0, 4095, 0, 359)+5); Serial.print(", ");
    Serial.print(map(mz, 0, 4095, 0, 359)-2); Serial.print(",");
    
    if (digitalRead(buttonPin01) == HIGH){
      Serial.print(1); Serial.print(","); 
    }
    else{
      Serial.print(0); Serial.print(",");
    }
    
    Serial.println(map(potentiometer, 0, 670, 0, 10));
    
    // blink LED to indicate activity
    blinkState = !blinkState;
    digitalWrite(LED_PIN, blinkState);
    
    Serial.flush();
    delay(1000);
}

long runningAverage(int M){
  static int LM[25];
  static byte index = 0;
  static long sum = 0;
  static byte count = 0;
  
  sum -= LM[index];
  LM[index] = M;
  sum += LM[index];
  index = index % LMSIZE;
  if (count < LMSIZE) count++;
  
  return sum / count;
}
