# ----------------------------------------#
# Autor:       Yannick Stibbe             #
# Department:  THGM-TL1                   #
# Email:       yannick.stibbe@airbus.com  # 
# Date:        11.08.2023                 # 
# ----------------------------------------#


import tkinter as tk
from tkinter import *
import serial
import serial.tools.list_ports_posix as list
import time

root = tk.Tk()
root.title("Slider GUI")

#Rudder
last_send1 = 0
#Rudder
last_send2 = 0




def selection_change(selection):
    # Serielle Verbindung initialisieren
    global ser
    ser = serial.Serial(selection, 9600)

def send_data(angle,index):
    combined_data = bytes([index, angle])
    ser.write(combined_data)
    x_label.config(text=f"Gesendet: {angle}")



# Drop Down Menu
options = []

for  port  in list.comports():
    options.append(port.device)

drop = OptionMenu(root,StringVar(),*options,command=selection_change)
drop.pack()


def update_rudder(event):
    global last_send1
    current_time1 = time.time()
    if current_time1 - last_send1 > 0.1:
        send_data(rudder.get(),0)
        last_send1 = time.time()

def update_engine(event):
    global last_send2
    current_time2 = time.time()
    if current_time2 - last_send2 > 0.2:
        value = engine1.get()
        send_data(value,1)
        last_send2 = time.time()


#Slider 
rudder = Scale(root, from_=0, to=180, orient=tk.HORIZONTAL, label="Rudder",command=update_rudder)
rudder.pack()
#Slider 
engine1 = Scale(root, from_=0, to=180, orient=tk.HORIZONTAL, label="Engine",command= update_engine)
engine1.pack()

#Label 
x_label = Label(root, text="Nichts Gesendet!")
x_label.pack()




root.mainloop()

# Serielle Verbindung schließen
ser.close()
