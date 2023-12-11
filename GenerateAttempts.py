import matplotlib.pyplot as plt
import pandas as pd
import numpy as np
import os

def generate_plot(csv_filename, table_count, keyCount):
    # Let's make sure our directories actually exist
    if not os.path.exists("Plots"):
        os.makedirs("Plots")
    if not os.path.exists("Plots/KeyAttempts"):
        os.makedirs("Plots/KeyAttempts")
    data = pd.read_csv(csv_filename)

    X = data['KeyCount'].tolist()
    Y = data['Attempts'].tolist()

    fig, ax = plt.subplots()
    plt.scatter(X, Y, label=f"Average Insertion Attempt for {table_count} Tables")

    # Processing the 'Attempts' column to calculate the mean
    data['Attempts'] = data['Attempts'].apply(lambda x: np.mean([int(i) for i in x.split('|')]))

    X = data['KeyCount'].tolist()
    Y = data['Attempts'].tolist()

    plt.scatter(X, Y, label=f"Average Insertion Attempt for {table_count} Tables, keyCount {keyCount}")
    plt.xlabel('Number of Keys')
    plt.ylabel('Insertion Attempts')
    plt.title(f"Average Key Insertion Attempts vs Number of Keys {keyCount} for {table_count} Tables")
    plt.grid()
    plt.legend()
    plt.savefig(f"Plots/KeyAttempts/attempts{keyCount}_{table_count}tables")

start_table_count = 2
end_table_count = 10
keyCount = [1000, 5000, 10000, 20000, 50000, 100000]

for i in range(start_table_count, end_table_count + 1):
    for j in keyCount:
        csv_filename = f'CSV_Files/KeyAttempts/attempts{j}_{i}Tables.csv'
        generate_plot(csv_filename, i, j)