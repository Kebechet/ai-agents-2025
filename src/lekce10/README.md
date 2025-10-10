# Reinforcement Learning - Q-learning on 2D Grid Environment

**Lekce 10 - AI Agenti**

## Description

This project implements a simple 2D grid environment with a Q-learning agent. The agent learns to navigate from a random starting position to a goal position at (4, 4) on a 5x5 grid.

## Environment

- **Grid Size**: 5x5
- **Agent**: Starts at a random position
- **Goal**: Position (4, 4)
- **Actions**: 4 discrete actions
  - 0: Move up
  - 1: Move down
  - 2: Move left
  - 3: Move right
- **Rewards**:
  - +1.0 for reaching the goal
  - -0.01 for each step (encourages shorter paths)
  - -1.0 for exceeding max steps (50)

## Q-Learning Algorithm

- **Learning Rate (α)**: 0.1
- **Discount Factor (γ)**: 0.9
- **Exploration Rate (ε)**: 0.1
- **Episodes**: 1000

## Project Structure

```
lekce10/
├── env.py           # 2D Grid environment implementation
├── setup.py         # Gymnasium environment registration
├── training.py      # Q-learning training script
├── main.py          # Testing the trained agent
├── requirements.txt # Dependencies
└── README.md        # This file
```

## Installation

1. Install dependencies:
```bash
pip install -r requirements.txt
```

## Usage

### 1. Train the Agent

Run the training script to train the Q-learning agent:

```bash
python training.py
```

This will:
- Train the agent for 1000 episodes
- Save the Q-table to `q_learning_q_table.npy`

### 2. Test the Trained Agent

After training, test the agent:

```bash
python main.py
```

This will:
- Load the trained Q-table
- Display the Q-values for each state
- Run one episode showing the agent's learned behavior

## How It Works

1. **Environment**: The agent navigates a 5x5 grid
2. **Q-Table**: Stores state-action values for all (state, action) pairs
3. **Training**: Uses epsilon-greedy policy to balance exploration and exploitation
4. **Q-Learning Update**: Off-policy temporal difference learning
   ```
   Q(s,a) ← Q(s,a) + α[r + γ max Q(s',a') - Q(s,a)]
   ```

## Results

After training, the agent learns the optimal path from any starting position to the goal at (4, 4).
