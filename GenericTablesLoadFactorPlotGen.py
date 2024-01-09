import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import mplcursors
import os


def average_probes(x):
    return 0.5 * (1 + (1/(1 - x)))


def average_probes_chaining(x):
    return 1 + x


def average_probes_double_hashing(x):
    return (-x ** -1) * np.log(1 - x)


def generate_plot(csv_filename, table_count, keyCount):
    data = pd.read_csv(csv_filename)

    X = data['LoadFactor'].tolist()
    Y = data['AverageLookUp'].tolist()
    table_sizes = [data[f'TableSize{i}'].tolist() for i in range(1, table_count + 1)]

    fig, ax = plt.subplots()
    ax.scatter(X, Y, label=f'Actual Average Lookup Time for {table_count} Tables')

    x = np.linspace(0, 0.99, 100)
    ax.plot(x, average_probes(x), 'g-', label='Average probes for linear probing')
    ax.plot(x, average_probes_chaining(x), 'r-', label="Average number of probes for chaining")
    ax.plot(x, average_probes_double_hashing(x), 'y-', label="Average number of probes for double hashing")

    labels = [''.join(f"table size {i+1}: {size},\n" for i, size in enumerate(sizes)) for sizes in zip(*table_sizes)]
    mplcursors.cursor(hover=True).connect("add", lambda sel: sel.annotation.set_text(labels[sel.index]))

    ax.set_xlabel('LoadFactor')
    ax.set_ylabel('Average Lookup Time')
    ax.set_title(f"Average Lookup Time vs Load Factor for {table_count} Tables, {keyCount * 2} (key) slots")
    ax.grid(True)
    ax.legend()

    # Ensure the Plots directory exists
    plot_dir = 'Plots'
    load_dir = os.path.join(plot_dir, 'LoadFactor')
    if not os.path.exists(load_dir):
        os.makedirs(load_dir)

    plot_filename = os.path.join(load_dir, f'GenericLoadFactor{keyCount}_{table_count}Tables.svg')
    plt.ylim(0, 5)
    plt.savefig(plot_filename)
    plt.close(fig)


start_table_count = 2
end_table_count = 10

keyCount = [1000, 5000, 10000, 20000, 50000, 100000]
#keyCount = [100000]

for i in range(start_table_count, end_table_count + 1):
    for j in keyCount:
        csv_filename = f'CSV_Files/LoadFactor/GenericLoadFactor{j}_{i}tables.csv'
        generate_plot(csv_filename, i, j)