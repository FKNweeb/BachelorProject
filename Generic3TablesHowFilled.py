import matplotlib.pyplot as plt
import pandas as pd
import numpy as np
import os

def generate_plot(csv_filename):
    if not os.path.exists("Plots"):
        os.makedirs("Plots")
    # Clear content before creating a new plot
    plt.clf()
    # Read the CSV file
    data = pd.read_csv(csv_filename)

    X = data['HowFilled'].tolist()
    Y = data['AverageLookup'].tolist()

    plt.scatter(X, Y, label=f"How filled the table is VS AverageLookup")
    plt.xlabel('How filled the table is')
    plt.ylabel('Average Lookup Time')
    plt.title(f"How filled the table is VS AverageLookup for 3 generic tables")
    plt.grid()
    plt.legend()
    plt.savefig(f"Plots/Generic3TablesHowFilled")


csv_filename = f'CSV_Files/Generic_3tablesHowFilled.csv'
generate_plot(csv_filename)