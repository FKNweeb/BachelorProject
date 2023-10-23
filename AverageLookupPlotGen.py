import matplotlib.pyplot as plt
import pandas as pd
import numpy as np

def generate_plot(csv_filename, table_count, expected_values):
    # Clear content before creating a new plot
    plt.clf()
    # Read the CSV file
    data = pd.read_csv(csv_filename)

    X = data['KeyCount'].tolist()
    Y = data['AverageLookup'].tolist()

    plt.scatter(X, Y, label=f"Average Lookup Attempt for {table_count} Tables")
    plt.plot(X, expected_values, label=f'Expected Average Lookup Time for {table_count} Tables', color='red')
    plt.xlabel('Number of Keys')
    plt.ylabel('Average Lookup Time')
    plt.title(f"Average Lookup Time vs Number of Keys for {table_count} Tables")
    plt.grid()
    plt.legend()
    plt.savefig(f"Plots/AverageLookup{table_count}Tables")
    plt.show()

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
    csv_filename = f'CSV_Files/averageLookup_{table_count}Tables.csv'
    generate_plot(csv_filename, table_count, expected_values)