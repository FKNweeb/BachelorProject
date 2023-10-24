import matplotlib.pyplot as plt
import pandas as pd
import numpy as np
import os

def generate_plot(csv_filename, table_count, expected_values):
    # Let's make sure our directories actually exist
    if not os.path.exists("Plots"):
        os.makedirs("Plots")
    if not os.path.exists("Plots/KeyAttempts"):
        os.makedirs("Plots/KeyAttempts")
    # Clear content before creating a new plot
    plt.clf()
    # Read the CSV file
    data = pd.read_csv(csv_filename)

    # Processing the 'Attempts' column to calculate the mean
    data['Attempts'] = data['Attempts'].apply(lambda x: np.mean([int(i) for i in x.split('|')]))

    X = data['KeyCount'].tolist()
    Y = data['Attempts'].tolist()

    plt.scatter(X, Y, label=f"Average Insertion Attempt for {table_count} Tables")
    plt.plot(X, expected_values, label=f'Expected Average Lookup Time for {table_count} Tables', color='red')
    plt.xlabel('Number of Keys')
    plt.ylabel('Insertion Attempts')
    plt.title(f"Average Key Insertion Attempts vs Number of Keys for {table_count} Tables")
    plt.grid()
    plt.legend()
    plt.savefig(f"Plots/KeyAttempts/KeyAttempts_{table_count}Tables")
    
    dist = np.linalg.norm(np.array(Y) - np.array(expected_values))
    print(dist)


expected_values_dict = {
    2: [1.5, 1.5, 1.5, 1.5, 1.5, 1.5],
    3: [2, 2, 2, 2, 2, 2],
    4: [2.5, 2.5, 2.5, 2.5, 2.5, 2.5],
    5: [3, 3, 3, 3, 3, 3],
    6: [3.5, 3.5, 3.5, 3.5, 3.5, 3.5],
    7: [4, 4, 4, 4, 4, 4],
    8: [4.5, 4.5, 4.5, 4.5, 4.5, 4.5],
    9: [5, 5, 5, 5, 5, 5],
    10: [5.5, 5.5, 5.5, 5.5, 5.5, 5.5]
}
for table_count, expected_values in expected_values_dict.items():
    csv_filename = f'CSV_Files/KeyAttempts/KeyAttempts_{table_count}Tables.csv'
    generate_plot(csv_filename, table_count, expected_values)